using DotLogix.WebServices.Core.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using IAuthenticationService = DotLogix.WebServices.Authentication.Services.IAuthenticationService;

namespace DotLogix.WebServices.Authentication.Extensions {
    public static class ServiceProviderExtensions {
        public static AuthenticationBuilder AddAuthentication<TService, TImplementation>(this IServiceCollection services, string schemeName, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TService : class, IAuthenticationService
            where TImplementation : class, TService {
            services.Add(typeof(TService), typeof(TImplementation), lifetime);
            services.AddAlias<IAuthenticationService, TService>(lifetime);

            var authenticationBuilder = services.AddAuthentication(schemeName);
            authenticationBuilder.AddScheme<AuthenticationServiceOptions, AuthenticationServiceHandler>(schemeName, c => { });

            return authenticationBuilder;
        }
    }
}
