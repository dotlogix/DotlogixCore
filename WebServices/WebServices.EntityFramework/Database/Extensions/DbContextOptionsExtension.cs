using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace DotLogix.WebServices.EntityFramework.Database.Extensions;

public sealed class DbContextOptionsExtension<TOptions> : IDbContextOptionsExtension
    where TOptions : IDbContextExtensionOptions, new()
{
    private ExtensionInfo _info;
    private TOptions _options;
    
    public ExtensionInfo Info => _info ??= new ExtensionInfo(this);
    DbContextOptionsExtensionInfo IDbContextOptionsExtension.Info => Info;

    public DbContextOptionsExtension() {
        _options = new TOptions();
    }

    private DbContextOptionsExtension(TOptions options) {
        _options = options;
    }

    public DbContextOptionsExtension<TOptions> WithOptions(Action<TOptions> configure) {
        var clone = (TOptions)_options.Clone();
        configure.Invoke(clone);
        return new DbContextOptionsExtension<TOptions>(clone);
    }

    public void ApplyServices(IServiceCollection services) {
        _options.ApplyServices(services);
    }

    public void Validate(IDbContextOptions options) {
        _options.Validate();
    }

    public sealed class ExtensionInfo : DbContextOptionsExtensionInfo {
        private string _logFragment;
        private int? _hashCode;
        public override DbContextOptionsExtension<TOptions> Extension => (DbContextOptionsExtension<TOptions>)base.Extension;
        public override bool IsDatabaseProvider => false;
        public override string LogFragment => _logFragment ??= Extension._options.CreateLogFragment();
        
        public ExtensionInfo(IDbContextOptionsExtension extension) : base(extension) {
        }

        public override int GetServiceProviderHashCode() {
            return _hashCode ??= Extension._options.GetServiceProviderHashCode();
        }

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other) {
            return other is ExtensionInfo info && Extension._options.ShouldUseSameServiceProvider(info.Extension._options);
        }

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo) {
            Extension._options.PopulateDebugInfo(debugInfo);
        }
    }
}