using System.Linq;
using System.Text;
using DotLogix.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DotLogix.WebServices.EntityFramework.Conventions; 

/// <summary>
///     A base class to implement <see cref="IDbNamingStrategy"/>
/// </summary>
public abstract class DbNamingStrategyBase : IDbNamingStrategy {
    /// <summary>
    /// The delimiter between name parts
    /// </summary>
    public string Delimiter { get; init; } = "_";

    /// <inheritdoc />
    public string GetDefaultSchemaName(IReadOnlyModel model) {
        var defaultSchema = model.GetDefaultSchema();
        return defaultSchema is not null ? RewriteSchemaName(defaultSchema) : default;
    }

    /// <inheritdoc />
    public virtual string GetTableSchemaName(IReadOnlyEntityType entityType) {
        var schemaName = entityType.GetSchema();
        return RewriteSchemaName(schemaName);
    }

    public string GetViewSchemaName(IReadOnlyEntityType entityType) {
        var schemaName = entityType.GetViewSchema();
        return RewriteSchemaName(schemaName);
    }

    /// <inheritdoc />
    public virtual string GetTableName(IReadOnlyEntityType entityType) {
        var typeName = entityType.GetTableName();
        return RewriteTableName(typeName);
    }

    public string GetViewName(IReadOnlyEntityType entityType) {
        var typeName = entityType.GetViewName();
        return RewriteViewName(typeName);
    }

    public string GetFunctionName(IReadOnlyEntityType entityType) {
        var typeName = entityType.GetFunctionName();
        return RewriteFunctionName(typeName);
    }

    public virtual string GetColumnName(IReadOnlyProperty property) {
        var tableStoreObject = StoreObjectIdentifier.Create(property.DeclaringEntityType, StoreObjectType.Table);
        return GetColumnName(property, tableStoreObject);
    }
    public virtual string GetColumnName(IReadOnlyProperty property, StoreObjectIdentifier? storeObject) {
        var propertyName = storeObject.HasValue ? property.GetColumnName(storeObject.Value) : property.GetColumnBaseName();
        return RewriteColumnName(propertyName);
    }

    /// <inheritdoc />
    public virtual string GetKeyName(IReadOnlyKey key) {
        var tableName = GetTableName(key.DeclaringEntityType);
        var columnNames = key.Properties.Select(GetColumnName);

        var isPrimary = key.IsPrimaryKey();
        var sb = new StringBuilder(isPrimary ? "pk" : "key");
        sb.Append(Delimiter)
           .Append(tableName)
           .Append(Delimiter, 2)
           .AppendJoin(Delimiter, columnNames);

        return sb.ToString();
    }

    /// <inheritdoc />
    public virtual string GetForeignKeyName(IReadOnlyForeignKey foreignKey) {
        var sourceTableName = GetTableName(foreignKey.DeclaringEntityType);
        var targetTableName = GetTableName(foreignKey.PrincipalEntityType);
        var sourceColumnNames = foreignKey.Properties.Select(GetColumnName);
        var targetColumnNames = foreignKey.PrincipalKey.Properties.Select(GetColumnName);
            
        var sb = new StringBuilder("fk");
        sb.Append(Delimiter)
           .Append(sourceTableName)
           .Append(Delimiter, 2)
           .Append(targetTableName)
           .Append(Delimiter, 2)
           .AppendJoin(Delimiter, sourceColumnNames)
           .Append(Delimiter, 2)
           .AppendJoin(Delimiter, targetColumnNames);

        return sb.ToString();
    }

    /// <inheritdoc />
    public virtual string GetIndexName(IReadOnlyIndex index) {
        var tableName = GetTableName(index.DeclaringEntityType);
        var columnNames = index.Properties.Select(GetColumnName);

        var sb = new StringBuilder(index.IsUnique ? "uniq" : "idx");
        sb.Append(Delimiter)
           .Append(tableName)
           .Append(Delimiter, 2)
           .AppendJoin(Delimiter, columnNames);

        return sb.ToString();
    }

    /// <inheritdoc />
    public abstract string Rewrite(string value);

    /// <inheritdoc />
    public virtual string RewriteSchemaName(string schemaName) {
        return Rewrite(schemaName);
    }

    /// <inheritdoc />
    public virtual string RewriteTableName(string tableName) {
        return Rewrite(tableName);
    }

    public string RewriteViewName(string viewName) {
        return Rewrite(viewName);
    }

    public string RewriteFunctionName(string functionName) {
        return Rewrite(functionName);
    }

    /// <inheritdoc />
    public virtual string RewriteColumnName(string columnName) {
        return Rewrite(columnName);
    }
}