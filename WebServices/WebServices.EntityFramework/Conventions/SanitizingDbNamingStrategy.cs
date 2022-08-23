#region
using System;
using DotLogix.Core.Extensions;
using DotLogix.Core.Utils.Naming;
using Microsoft.EntityFrameworkCore;
#endregion

namespace DotLogix.WebServices.EntityFramework.Conventions; 

/// <summary>
/// An implementation of the <see cref="IDbNamingStrategy"/> which removes pre- and suffixes of table names
/// </summary>
public class SanitizingDbNamingStrategy : DbNamingStrategy {
    /// <summary>
    /// The prefix to omit when rewriting schema names
    /// </summary>
    public string SchemaPrefix { get; init; }
    /// <summary>
    /// The suffix to omit when rewriting schema names
    /// </summary>
    public string SchemaSuffix { get; init; } = nameof(DbContext);
        
    /// <summary>
    /// The prefix to omit when rewriting table names
    /// </summary>
    public string TablePrefix { get; init; }
    /// <summary>
    /// The suffix to omit when rewriting table names
    /// </summary>
    public string TableSuffix { get; init; } = "Entity";
        
    /// <summary>
    /// The prefix to omit when rewriting column names
    /// </summary>
    public string ColumnPrefix { get; init; }
    /// <summary>
    /// The suffix to omit when rewriting column names
    /// </summary>
    public string ColumnSuffix { get; init; }
        
        
    /// <summary>
    /// Creates a new instance of <see cref="SanitizingDbNamingStrategy"/>
    /// </summary>
    public SanitizingDbNamingStrategy(INamingStrategy namingStrategy) : base(namingStrategy)
    {
    }

    /// <inheritdoc />
    public override string RewriteSchemaName(string schemaName) {
        return base.RewriteSchemaName(schemaName?.Trim(SchemaPrefix, SchemaSuffix));
    }

    /// <inheritdoc />
    public override string RewriteTableName(string tableName) {
        return base.RewriteTableName(tableName?.Trim(TablePrefix, TableSuffix));
    }

    /// <inheritdoc />
    public override string RewriteColumnName(string columnName) {
        return base.RewriteColumnName(columnName?.Trim(ColumnPrefix, ColumnSuffix));
    }
        
    protected bool Equals(SanitizingDbNamingStrategy other) {
        return base.Equals(other)
         && SchemaPrefix == other.SchemaPrefix
         && SchemaSuffix == other.SchemaSuffix
         && TablePrefix == other.TablePrefix
         && TableSuffix == other.TableSuffix
         && ColumnPrefix == other.ColumnPrefix
         && ColumnSuffix == other.ColumnSuffix;
    }

    public override bool Equals(object obj) {
        if(ReferenceEquals(null, obj)) {
            return false;
        }

        if(ReferenceEquals(this, obj)) {
            return true;
        }

        if(obj.GetType() != GetType()) {
            return false;
        }

        return Equals((SanitizingDbNamingStrategy)obj);
    }

    public override int GetHashCode() {
        var hashCode = new HashCode();
        hashCode.Add(base.GetHashCode());
        hashCode.Add(SchemaPrefix);
        hashCode.Add(SchemaSuffix);
        hashCode.Add(TablePrefix);
        hashCode.Add(TableSuffix);
        hashCode.Add(ColumnPrefix);
        hashCode.Add(ColumnSuffix);
        return hashCode.ToHashCode();
    }
}