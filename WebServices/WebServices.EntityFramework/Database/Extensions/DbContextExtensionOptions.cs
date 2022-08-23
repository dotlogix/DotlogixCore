using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotLogix.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace DotLogix.WebServices.EntityFramework.Database; 

public abstract class DbContextExtensionOptions : IDbContextExtensionOptions {
    void IDbContextExtensionOptions.ApplyServices(IServiceCollection services) => ApplyServices(services);
    void IDbContextExtensionOptions.PopulateDebugInfo(IDictionary<string, string> debugInfo) => PopulateDebugInfo(debugInfo);
    void IDbContextExtensionOptions.Validate() => Validate();
    int IDbContextExtensionOptions.GetServiceProviderHashCode() => GetServiceProviderHashCode();
    bool IDbContextExtensionOptions.ShouldUseSameServiceProvider(IDbContextExtensionOptions other) => ShouldUseSameServiceProvider(other);
    string IDbContextExtensionOptions.CreateLogFragment() => CreateLogFragment();
    IDbContextExtensionOptions IDbContextExtensionOptions.Clone() => Clone();

    protected abstract void ApplyServices(IServiceCollection services);
    protected abstract void PopulateDebugInfo(IDictionary<string, string> debugInfo);
    protected virtual void Validate(){}
    
    protected abstract int GetServiceProviderHashCode();
    protected abstract bool ShouldUseSameServiceProvider(IDbContextExtensionOptions options);
    protected virtual string CreateLogFragment() {
        var debugInfo = new Dictionary<string, string>();
        PopulateDebugInfo(debugInfo);

        return new StringBuilder(GetType().GetFriendlyName())
           .Append(" = { ")
           .AppendJoin(", ",
                debugInfo
                   .OrderBy(kv => kv.Key)
                   .Select(kv => $"{kv.Key}={kv.Value}")
            )
           .Append(" }")
           .ToString();
    }
    protected virtual IDbContextExtensionOptions Clone() => (IDbContextExtensionOptions)MemberwiseClone();
}