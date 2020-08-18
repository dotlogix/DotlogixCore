using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using DotLogix.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DotLogix.Architecture.Infrastructure.EntityFramework.Conventions {
    public abstract class DbNamingStrategyBase : IDbNamingStrategy {
        protected char Delimiter { get; } = '_';

        public virtual string GetSchemaName(IEntityType entityType) {
            var attribute = entityType.ClrType.GetCustomAttribute<TableAttribute>();
            return RewriteSchemaName(attribute?.Schema);
        }

        public virtual string GetTableName(IEntityType entityType) {
            var attribute = entityType.ClrType.GetCustomAttribute<TableAttribute>();
            var typeName = attribute?.Name ?? entityType.ClrType.Name;

            return RewriteTableName(typeName);
        }

        public virtual string GetColumnName(IProperty property) {
            var attribute = property.PropertyInfo.GetCustomAttribute<ColumnAttribute>();
            return RewriteColumnName(attribute?.Name ?? property.PropertyInfo.Name);
        }

        public virtual string GetKeyName(IKey key) {
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

        public virtual string GetForeignKeyName(IForeignKey foreignKey) {
            var sourceTableName = GetTableName(foreignKey.DeclaringEntityType);
            var targetTableName = GetTableName(foreignKey.PrincipalEntityType);
            var sourceColumnNames = foreignKey.Properties.Select(GetColumnName);

            var sb = new StringBuilder("fk");
            sb.Append(Delimiter)
              .Append(sourceTableName)
              .Append(Delimiter, 2)
              .Append(targetTableName)
              .Append(Delimiter, 2)
              .AppendJoin(Delimiter, sourceColumnNames);

            return sb.ToString();
        }

        public virtual string GetIndexName(IIndex index) {
            var tableName = GetTableName(index.DeclaringEntityType);
            var columnNames = index.Properties.Select(GetColumnName);

            var sb = new StringBuilder(index.IsUnique ? "uniq" : "idx");
            sb.Append(Delimiter)
              .Append(tableName)
              .Append(Delimiter, 2)
              .AppendJoin(Delimiter, columnNames);

            return sb.ToString();
        }

        public abstract string Rewrite(string value);

        public virtual string RewriteSchemaName(string schemaName) {
            return Rewrite(schemaName);
        }

        public virtual string RewriteTableName(string tableName) {
            return Rewrite(tableName);
        }

        public virtual string RewriteColumnName(string columnName) {
            return Rewrite(columnName);
        }
    }
}