using DotLogix.Core.Diagnostics;
using DotLogix.WebServices.Adapters.Endpoints;
using DotLogix.WebServices.Adapters.Http;
using DotLogix.WebServices.Core.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DotLogix.WebServices.Testing.AspCore; 

public class WebServerFactory<TStartup>: WebApplicationFactory<TStartup> where TStartup : class
{
    public IConfiguration Configuration { get; }
    public IServiceCollection ServiceOverrides { get; }

    public WebServerFactory(IConfiguration configuration, IServiceCollection serviceOverrides)
    {
        Configuration = configuration;
        ServiceOverrides = serviceOverrides;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        if(Configuration is not null)
        {
            builder.UseConfiguration(Configuration);
        }

        if(ServiceOverrides is not null)
        {
            builder.ConfigureTestServices(OverrideServices);
        }
    }

    private void OverrideServices(IServiceCollection services) {
        foreach (var service in ServiceOverrides)
        {
            services.RemoveAll(service.ServiceType);
            services.Add(service);
        }
            
        services.RemoveAll(typeof(ILogTarget));
        services.RemoveAll(typeof(IAsyncLogTarget));
        services.AddLogTarget<NUnitLogTarget>();
            
        services.RemoveAll(typeof(IWebServiceEndpoint));
        services.RemoveAll(typeof(IHttpProvider));

        services.AddScoped<IHttpProvider>(_ => new HttpProvider(CreateClient()));
        services.AddScoped<IWebServiceEndpoint>(_ => new StaticWebServiceEndpoint(ClientOptions.BaseAddress));
    }
}