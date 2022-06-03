using System;
using System.Collections.Generic;
using DotLogix.Core.Utils.Naming;
using DotLogix.WebServices.EntityFramework.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DotLogix.WebServices.EntityFramework.Database.Extensions;

public class NamingConventionsExtensionOptions : DbContextExtensionOptions {
    private INamingStrategy _namingStrategy;
    private string _schemaPrefix;
    private string _schemaSuffix = nameof(DbContext);
    private string _tablePrefix;
    private string _tableSuffix = "Entity";
    private string _columnPrefix;
    private string _columnSuffix;

    public NamingConventionsExtensionOptions UseNamingStrategy(INamingStrategy namingStrategy) {
        _namingStrategy = namingStrategy;
        return this;
    }

    public NamingConventionsExtensionOptions UseSchemaPrefix(string prefix) {
        _schemaPrefix = prefix;
        return this;
    }
    public NamingConventionsExtensionOptions UseSchemaSuffix(string suffix) {
        _schemaSuffix = suffix;
        return this;
    }

    public NamingConventionsExtensionOptions UseTablePrefix(string prefix) {
        _tablePrefix = prefix;
        return this;
    }
    public NamingConventionsExtensionOptions UseTableSuffix(string suffix) {
        _tableSuffix = suffix;
        return this;
    }
    
    public NamingConventionsExtensionOptions UseColumnPrefix(string prefix) {
        _columnPrefix = prefix;
        return this;
    }
    public NamingConventionsExtensionOptions UseColumnSuffix(string suffix) {
        _columnSuffix = suffix;
        return this;
    }
    
    #region Option Callbacks
    protected override void ApplyServices(IServiceCollection services) {
        services.TryAddSingleton<IDbNamingStrategy>(new SanitizingDbNamingStrategy(_namingStrategy) {
            SchemaPrefix = _schemaPrefix,
            SchemaSuffix = _schemaSuffix,
            TablePrefix = _tablePrefix,
            TableSuffix = _tableSuffix,
            ColumnPrefix = _columnPrefix,
            ColumnSuffix = _columnSuffix
        });
        
        new EntityFrameworkRelationalServicesBuilder(services)
           .TryAdd<IConventionSetPlugin, NamingStrategyConventionSetPlugin>();
    }
    protected override void PopulateDebugInfo(IDictionary<string, string> debugInfo) {
        debugInfo["NamingStrategy"] = $"Sanitizing({_namingStrategy})";
    }
    protected override int GetServiceProviderHashCode() {
        var hashCode = new HashCode();
        hashCode.Add(_namingStrategy);
        hashCode.Add(_schemaPrefix);
        hashCode.Add(_schemaSuffix);
        hashCode.Add(_tablePrefix);
        hashCode.Add(_tableSuffix);
        hashCode.Add(_columnPrefix);
        hashCode.Add(_columnSuffix);
        return hashCode.ToHashCode();
    }
    protected override bool ShouldUseSameServiceProvider(IDbContextExtensionOptions options) {
        return options is NamingConventionsExtensionOptions other
         && Equals(_namingStrategy, other._namingStrategy)
         && _schemaPrefix == other._schemaPrefix
         && _schemaSuffix == other._schemaSuffix
         && _tablePrefix == other._tablePrefix
         && _tableSuffix == other._tableSuffix
         && _columnPrefix == other._columnPrefix
         && _columnSuffix == other._columnSuffix;
    }
    protected override IDbContextExtensionOptions Clone() {
        return new NamingConventionsExtensionOptions {
            _namingStrategy = _namingStrategy
        };
    }
    #endregion
}