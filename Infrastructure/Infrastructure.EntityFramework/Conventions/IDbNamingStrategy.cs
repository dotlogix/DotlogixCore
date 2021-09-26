using Microsoft.EntityFrameworkCore.Metadata;

namespace DotLogix.Infrastructure.EntityFramework.Conventions {
    public interface IDbNamingStrategy {
        string GetSchemaName(IEntityType entityType);
        string GetTableName(IEntityType entityType);
        string GetColumnName(IProperty property);
        string GetKeyName(IKey key);
        string GetForeignKeyName(IForeignKey foreignKey);
        string GetIndexName(IIndex index);
        string Rewrite(string value);
        string RewriteSchemaName(string schemaName);
        string RewriteTableName(string tableName);
        string RewriteColumnName(string columnName);
    }
}