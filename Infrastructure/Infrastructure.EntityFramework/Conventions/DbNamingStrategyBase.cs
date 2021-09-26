using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using DotLogix.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DotLogix.Infrastructure.EntityFramework.Conventions {
    /// <summary>
    ///     A base class to implement <see cref="IDbNamingStrategy"/>
    /// </summary>
    public abstract class DbNamingStrategyBase : IDbNamingStrategy {
        /// <summary>
        /// The delimiter between name parts
        /// </summary>
        protected string Delimiter { get; }

        /// <summary>
        /// Creates a new instance of <see cref="DbNamingStrategyBase"/>
        /// </summary>
        protected DbNamingStrategyBase() {
            Delimiter = "_";
        }
        
        /// <summary>
        /// Creates a new instance of <see cref="DbNamingStrategyBase"/>
        /// </summary>
        protected DbNamingStrategyBase(string delimiter) {
            Delimiter = delimiter;
        }

        /// <inheritdoc />
        public virtual string GetSchemaName(IEntityType entityType) {
            var attribute = entityType.ClrType.GetCustomAttribute<TableAttribute>();
            var schemaName = attribute?.Schema ?? entityType.GetSchema();
            return RewriteSchemaName(schemaName);
        }

        /// <inheritdoc />
        public virtual string GetTableName(IEntityType entityType) {
            var attribute = entityType.ClrType.GetCustomAttribute<TableAttribute>();
            var typeName = attribute?.Name ?? entityType.GetTableName();
            return RewriteTableName(typeName);
        }

        /// <inheritdoc />
        public virtual string GetColumnName(IProperty property) {
            var attribute = property.PropertyInfo.GetCustomAttribute<ColumnAttribute>();
            var columnName = attribute != null ? attribute.Name : property.GetColumnName();
            return RewriteColumnName(columnName);
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public virtual string RewriteColumnName(string columnName) {
            return Rewrite(columnName);
        }
    }
}