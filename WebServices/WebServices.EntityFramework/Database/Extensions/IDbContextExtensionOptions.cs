using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace DotLogix.WebServices.EntityFramework.Database; 

public interface IDbContextExtensionOptions
{
    void ApplyServices(IServiceCollection services);
    void PopulateDebugInfo(IDictionary<string, string> debugInfo);
    void Validate();
    int GetServiceProviderHashCode();
    bool ShouldUseSameServiceProvider(IDbContextExtensionOptions other);
    string CreateLogFragment();
    IDbContextExtensionOptions Clone();
}