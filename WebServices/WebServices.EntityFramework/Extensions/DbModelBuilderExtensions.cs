using System;
using System.Linq.Expressions;
using DotLogix.Infrastructure.EntityFramework.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotLogix.WebServices.EntityFramework.Extensions {
    public static class DbModelBuilderExtensions {
        public static void ApplyNamingStrategy(this ModelBuilder builder, IDbNamingStrategy namingStrategy) {
            var schemaName = namingStrategy.RewriteSchemaName(builder.Model.GetDefaultSchema());
            builder.Model.SetDefaultSchema(schemaName);

            foreach(var entityType in builder.Model.GetEntityTypes()) {
                entityType.SetTableName(namingStrategy.GetTableName(entityType));

                foreach(var property in entityType.GetProperties()) {
                    property.SetColumnName(namingStrategy.GetColumnName(property));
                }
                
                foreach(var key in entityType.GetKeys()) {
                    key.SetName(namingStrategy.GetKeyName(key));
                }

                foreach(var foreignKey in entityType.GetForeignKeys()) {
                    foreignKey.SetConstraintName(namingStrategy.GetForeignKeyName(foreignKey));
                }

                foreach(var index in entityType.GetIndexes()) {
                    index.SetDatabaseName(namingStrategy.GetIndexName(index));
                }
            }
        }
        
        
        public static ReferenceCollectionBuilder<TOne, TMany> HasManyToOneRelation<TMany, TOne>(this EntityTypeBuilder<TMany> builder,
                                                                                                                Expression<Func<TMany, object>> foreignKeySelector,
                                                                                                                Expression<Func<TOne, object>> principleKeySelector)
            where TMany : class
            where TOne : class {
            return builder
                  .HasOne<TOne>()
                  .WithMany()
                  .HasForeignKey(foreignKeySelector)
                  .HasPrincipalKey(principleKeySelector);
        }

        public static ReferenceCollectionBuilder<TOne, TMany> HasOneToManyRelation<TOne, TMany>(this EntityTypeBuilder<TOne> builder,
                                                                                                                Expression<Func<TMany, object>> foreignKeySelector,
                                                                                                                Expression<Func<TOne, object>> principleKeySelector)
            where TMany : class
            where TOne : class {
            return builder
                  .HasMany<TMany>()
                  .WithOne()
                  .HasForeignKey(foreignKeySelector)
                  .HasPrincipalKey(principleKeySelector);
        }

        public static ReferenceReferenceBuilder<TPrincipal, TChild> HasOneToOneRelation<TPrincipal, TChild>(this EntityTypeBuilder<TPrincipal> builder,
                                                                                                              Expression<Func<TPrincipal, object>> principalKeySelector,
                                                                                                              Expression<Func<TChild, object>> keySelector, bool required = true)
            where TPrincipal : class
            where TChild : class {
            return builder
                  .HasOne<TChild>()
                  .WithOne()
                  .HasForeignKey(principalKeySelector).IsRequired(required)
                  .HasPrincipalKey(keySelector).IsRequired(required);
        }

        public static IndexBuilder<TEntity> HasIndex<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, object>> keySelectorExpression, bool isUnique = false)
            where TEntity : class {
            return builder
                  .HasIndex(keySelectorExpression)
                  .IsUnique(isUnique);
        }

        public static IndexBuilder<TEntity> HasUniqueIndex<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, object>> keySelectorExpression)
            where TEntity : class {
            return builder
                  .HasIndex(keySelectorExpression)
                  .IsUnique();
        }
    }
}
