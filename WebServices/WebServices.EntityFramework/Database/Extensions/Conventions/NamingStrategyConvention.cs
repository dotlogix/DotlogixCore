using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.WebServices.EntityFramework.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DotLogix.WebServices.EntityFramework.Database; 

public class NamingStrategyConvention :
    IEntityTypeAddedConvention,
    IEntityTypeAnnotationChangedConvention,
    IEntityTypeBaseTypeChangedConvention,
    IEntityTypePrimaryKeyChangedConvention,
    IPropertyAddedConvention,
    
    IForeignKeyAddedConvention,
    IForeignKeyPropertiesChangedConvention,
    IForeignKeyOwnershipChangedConvention,
    
    IKeyAddedConvention,
    IIndexAddedConvention,
    IIndexUniquenessChangedConvention,
    
    IModelFinalizingConvention {
    private static readonly StoreObjectType[] StoreObjectTypes = { StoreObjectType.Table, StoreObjectType.View, StoreObjectType.Function, StoreObjectType.SqlQuery };
    private readonly IDbNamingStrategy _namingStrategy;

    public NamingStrategyConvention(IDbNamingStrategy nameRewriter) {
        _namingStrategy = nameRewriter;
    }

    public virtual void ProcessEntityTypeAdded(
        IConventionEntityTypeBuilder entityTypeBuilder,
        IConventionContext<IConventionEntityTypeBuilder> context
    ) {
        var entityType = entityTypeBuilder.Metadata;

        // Note that the base type is null when the entity type is first added - a base type only gets added later
        // (see ProcessEntityTypeBaseTypeChanged). But we still have this check for safety.
        if(entityType.BaseType is null) {
            entityTypeBuilder.ToTable(_namingStrategy.GetTableName(entityType), _namingStrategy.GetTableSchemaName(entityType));

            if(entityType.GetViewNameConfigurationSource() == ConfigurationSource.Convention) {
                entityTypeBuilder.ToView(_namingStrategy.GetViewName(entityType), _namingStrategy.GetViewSchemaName(entityType));
            }
        }
    }

    public void ProcessEntityTypeAnnotationChanged(
        IConventionEntityTypeBuilder entityTypeBuilder,
        string name,
        IConventionAnnotation annotation,
        IConventionAnnotation oldAnnotation,
        IConventionContext<IConventionAnnotation> context
    ) {
        var entityType = entityTypeBuilder.Metadata;

        // If the View/SqlQuery/Function name is being set on the entity type, and its table name is set by convention, then we assume
        // we're the one who set the table name back when the entity type was originally added. We now undo this as the entity type
        // should only be mapped to the View/SqlQuery/Function.
        if(name is RelationalAnnotationNames.ViewName or RelationalAnnotationNames.SqlQuery or RelationalAnnotationNames.FunctionName && annotation.Value is not null && (entityType.GetTableNameConfigurationSource() == ConfigurationSource.Convention)) {
            entityType.SetTableName(null);
        }

        if((name != RelationalAnnotationNames.TableName) || StoreObjectIdentifier.Create(entityType, StoreObjectType.Table) is not { } tableIdentifier) {
            return;
        }

        // The table's name is changing - rewrite keys, index names

        if(entityType.FindPrimaryKey() is { } primaryKey) {
            // We need to rewrite the PK name.
            // However, this isn't yet supported with TPT, see https://github.com/dotnet/efcore/issues/23444.
            // So we need to check if the entity is within a TPT hierarchy, or is an owned entity within a TPT hierarchy.

            var rootType = entityType.GetRootType();
            var isTpt = rootType.GetDerivedTypes().FirstOrDefault() is { } derivedType && (derivedType.GetTableName() != rootType.GetTableName());

            if(entityType.FindRowInternalForeignKeys(tableIdentifier).FirstOrDefault() is null && !isTpt) {
                primaryKey.Builder.HasName(_namingStrategy.GetKeyName(primaryKey));
            } else {
                // This hierarchy is being transformed into TPT via the explicit setting of the table name.
                // We not only have to reset our own key name, but also the parents'. Otherwise, the parent's key name
                // is used as the child's (see RelationalKeyExtensions.GetName), and we get a "duplicate key name in database" error
                // since both parent and child have the same key name;
                foreach(var type in entityType.GetRootType().GetDerivedTypesInclusive()) {
                    if(type.FindPrimaryKey() is { } pk) {
                        pk.Builder.HasNoAnnotation(RelationalAnnotationNames.Name);
                    }
                }
            }
        }

        foreach(var foreignKey in entityType.GetForeignKeys()) {
            foreignKey.Builder.HasConstraintName(_namingStrategy.GetForeignKeyName(foreignKey));
        }

        foreach(var index in entityType.GetIndexes()) {
            index.Builder.HasDatabaseName(_namingStrategy.GetIndexName(index));
        }

        if(annotation?.Value is not null && entityType.FindOwnership() is { } ownership && ((string)annotation.Value != ownership.PrincipalEntityType.GetTableName())) {
            // An owned entity's table is being set explicitly - this is the trigger to undo table splitting (which is the default).

            // When the entity became owned, we prefixed all of its properties - we must now undo that.
            foreach(var property in entityType.GetProperties()
               .Except(entityType.FindPrimaryKey()!.Properties)
               .Where(p => p.Builder.CanSetColumnName(null))) {
                RewriteColumnName(property.Builder);
            }

            // We previously rewrote the owned entity's primary key name, when the owned entity was still in table splitting.
            // Now that its getting its own table, rewrite the primary key constraint name again.
            if(entityType.FindPrimaryKey() is { } key) {
                key.Builder.HasName(_namingStrategy.GetKeyName(key));
            }
        }
    }

    public void ProcessEntityTypeBaseTypeChanged(
        IConventionEntityTypeBuilder entityTypeBuilder,
        IConventionEntityType newBaseType,
        IConventionEntityType oldBaseType,
        IConventionContext<IConventionEntityType> context
    ) {
        var entityType = entityTypeBuilder.Metadata;

        if(newBaseType is null) {
            // The entity is getting removed from a hierarchy. Set the (rewritten) TableName.
            entityTypeBuilder.ToTable(_namingStrategy.GetTableName(entityType), _namingStrategy.GetTableSchemaName(entityType));
        } else {
            // The entity is getting a new base type (e.g. joining a hierarchy).
            // If this is TPH, we remove the previously rewritten TableName (and non-rewritten Schema) which we set when the
            // entity type was first added to the model (see ProcessEntityTypeAdded).
            // If this is TPT, TableName and Schema are set explicitly, so the following will be ignored.
            entityTypeBuilder.HasNoAnnotation(RelationalAnnotationNames.TableName);
            entityTypeBuilder.HasNoAnnotation(RelationalAnnotationNames.Schema);
        }
    }

    public void ProcessEntityTypePrimaryKeyChanged(IConventionEntityTypeBuilder entityTypeBuilder, IConventionKey newPrimaryKey, IConventionKey previousPrimaryKey, IConventionContext<IConventionKey> context) {
        newPrimaryKey?.SetName(_namingStrategy.GetKeyName(newPrimaryKey));
        previousPrimaryKey?.SetName(_namingStrategy.GetKeyName(previousPrimaryKey));
    }

    public virtual void ProcessPropertyAdded(
        IConventionPropertyBuilder propertyBuilder,
        IConventionContext<IConventionPropertyBuilder> context
    ) {
        RewriteColumnName(propertyBuilder);
    }

    public void ProcessForeignKeyAdded(
        IConventionForeignKeyBuilder relationshipBuilder,
        IConventionContext<IConventionForeignKeyBuilder> context
    ) {
        relationshipBuilder.HasConstraintName(_namingStrategy.GetForeignKeyName(relationshipBuilder.Metadata));
    }

    public void ProcessForeignKeyPropertiesChanged(
        IConventionForeignKeyBuilder relationshipBuilder,
        IReadOnlyList<IConventionProperty> oldDependentProperties,
        IConventionKey oldPrincipalKey,
        IConventionContext<IReadOnlyList<IConventionProperty>> context
    ) {
        relationshipBuilder.HasConstraintName(_namingStrategy.GetForeignKeyName(relationshipBuilder.Metadata));
    }

    public void ProcessForeignKeyOwnershipChanged(
        IConventionForeignKeyBuilder relationshipBuilder,
        IConventionContext<bool?> context
    ) {
        var foreignKey = relationshipBuilder.Metadata;
        var ownedEntityType = foreignKey.DeclaringEntityType;

        // An entity type is becoming owned - this is a bit complicated.
        // Unless it's a collection navigation, or the owned entity table name was explicitly set by the user, this triggers table
        // splitting, which means we need to undo rewriting which we've done previously.
        if(foreignKey.IsOwnership && !foreignKey.GetNavigation(false)!.IsCollection && (ownedEntityType.GetTableNameConfigurationSource() != ConfigurationSource.Explicit)) {
            // Reset the table name which we've set when the entity type was added.
            // If table splitting was configured by explicitly setting the table name, the following
            // does nothing.
            ownedEntityType.Builder.HasNoAnnotation(RelationalAnnotationNames.TableName);
            ownedEntityType.Builder.HasNoAnnotation(RelationalAnnotationNames.Schema);

            ownedEntityType.FindPrimaryKey()?.Builder.HasNoAnnotation(RelationalAnnotationNames.Name);

            // We've previously set rewritten column names when the entity was originally added (before becoming owned).
            // These need to be rewritten again to include the owner prefix.
            foreach(var property in ownedEntityType.GetProperties()) {
                RewriteColumnName(property.Builder);
            }
        }
    }


    public void ProcessIndexAdded(
        IConventionIndexBuilder indexBuilder,
        IConventionContext<IConventionIndexBuilder> context
    ) {
        indexBuilder.HasDatabaseName(_namingStrategy.GetIndexName(indexBuilder.Metadata));
    }

    public void ProcessIndexUniquenessChanged(
        IConventionIndexBuilder indexBuilder,
        IConventionContext<bool?> context
    ) {
        indexBuilder.HasDatabaseName(_namingStrategy.GetIndexName(indexBuilder.Metadata));
    }

    public void ProcessKeyAdded(
        IConventionKeyBuilder keyBuilder,
        IConventionContext<IConventionKeyBuilder> context
    ) {
        keyBuilder.HasName(_namingStrategy.GetKeyName(keyBuilder.Metadata));
    }

    /// <summary>
    ///     EF Core's <see cref="SharedTableConvention" /> runs at model finalization time, and adds entity type prefixes to
    ///     clashing columns. These prefixes also needs to be rewritten by us, so we run after that convention to do that.
    /// </summary>
    public void ProcessModelFinalizing(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context) {
        foreach(var entityType in modelBuilder.Metadata.GetEntityTypes()) {
            foreach(var property in entityType.GetProperties()) {
                var shortTypeName = entityType.ShortName();
                var columnName = property.GetColumnBaseName();
                if(columnName.StartsWith(shortTypeName + '_', StringComparison.Ordinal)) {
                    var tableDiscriminator = _namingStrategy.Rewrite(shortTypeName);
                    var uniqueTableName = string.Concat(tableDiscriminator, columnName.AsSpan(shortTypeName.Length));
                    property.Builder.HasColumnName(uniqueTableName);
                }

                foreach(var storeObjectType in StoreObjectTypes) {
                    var identifier = StoreObjectIdentifier.Create(entityType, storeObjectType);
                    if(identifier is null) {
                        continue;
                    }

                    if(property.GetColumnNameConfigurationSource(identifier.Value) != ConfigurationSource.Convention) {
                        continue;
                    }

                    columnName = property.GetColumnName(identifier.Value);
                    if(columnName!.StartsWith(shortTypeName + '_', StringComparison.Ordinal)) {
                        var tableDiscriminator = _namingStrategy.Rewrite(shortTypeName);
                        var uniqueTableName = string.Concat(tableDiscriminator, columnName.AsSpan(shortTypeName.Length));
                        property.Builder.HasColumnName(uniqueTableName);
                    }
                }
            }
        }
    }

    private void RewriteColumnName(IConventionPropertyBuilder propertyBuilder) {
        var property = propertyBuilder.Metadata;
        var entityType = property.DeclaringEntityType;

        // Remove any previous setting of the column name we may have done, so we can get the default recalculated below.
        property.Builder.HasNoAnnotation(RelationalAnnotationNames.ColumnName);

        // TODO: The following is a temporary hack. We should probably just always set the relational override below,
        // but https://github.com/dotnet/efcore/pull/23834
        propertyBuilder.HasColumnName(_namingStrategy.GetColumnName(propertyBuilder.Metadata));

        foreach(var storeObjectType in StoreObjectTypes) {
            var identifier = StoreObjectIdentifier.Create(entityType, storeObjectType);
            if(identifier.HasValue == false) {
                continue;
            }

            if(property.GetColumnNameConfigurationSource(identifier.Value) == ConfigurationSource.Convention) {
                propertyBuilder.HasColumnName(_namingStrategy.GetColumnName(property, identifier.Value), identifier.Value);
            }
        }
    }
}