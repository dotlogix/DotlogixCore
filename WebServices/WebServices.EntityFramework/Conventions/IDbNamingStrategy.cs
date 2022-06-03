using Microsoft.EntityFrameworkCore.Metadata;

namespace DotLogix.WebServices.EntityFramework.Conventions {
    public interface IDbNamingStrategy {
        string GetDefaultSchemaName(IReadOnlyModel model);
        string GetViewSchemaName(IReadOnlyEntityType entityType);
        string GetTableSchemaName(IReadOnlyEntityType entityType);
        
        string GetTableName(IReadOnlyEntityType entityType);
        string GetViewName(IReadOnlyEntityType entityType);
        string GetFunctionName(IReadOnlyEntityType entityType);
        
        string GetColumnName(IReadOnlyProperty property);
        string GetColumnName(IReadOnlyProperty property, StoreObjectIdentifier? storeObject);
        
        string GetKeyName(IReadOnlyKey key);
        string GetForeignKeyName(IReadOnlyForeignKey foreignKey);
        string GetIndexName(IReadOnlyIndex index);
        
        string RewriteSchemaName(string schemaName);
        string RewriteTableName(string tableName);
        string RewriteViewName(string viewName);
        string RewriteFunctionName(string functionName);

        string RewriteColumnName(string columnName);
        
        string Rewrite(string value);
    }
}