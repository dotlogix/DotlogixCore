using System;
using DotLogix.WebServices.Adapters.Endpoints;
using DotLogix.WebServices.Adapters.Http;
using DotLogix.WebServices.Testing.AspCore.Mocks;
using DotLogix.WebServices.Testing.EntityFramework;
using Microsoft.Extensions.DependencyInjection;

namespace DotLogix.WebServices.Testing.AspCore
{
    public abstract class WebServiceTestBase<TStartup> : EfTestBase
        where TStartup : class
    {
        protected MockWebServiceEndpoint Endpoint { get; } = new MockWebServiceEndpoint();
        protected HttpProvider HttpProvider { get; private set; }

        protected override void ConfigureServices(ServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddSingleton<IHttpProvider>(_ => HttpProvider);
            services.AddSingleton<IWebServiceEndpoints>(_ => new MockWebServiceEndpoints {
                CurrentEndpointType = EndpointType.Local
            });
        }

        protected override IServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            var factory = new WebServerFactory<TStartup>(Configuration, services);
            HttpProvider = CreateHttpProvider(factory);

            Endpoint.UseUri(factory.ClientOptions.BaseAddress);
            factory.CreateDefaultClient();

            return factory.Server.Host.Services;
        }

        protected virtual HttpProvider CreateHttpProvider(WebServerFactory<TStartup> factory)
        {
            return new HttpProvider(factory.CreateClient(), AuthenticationTokenProvider);
        }
    }
}
