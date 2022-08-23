using DotLogix.WebServices.Core.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using IAuthenticationService = DotLogix.WebServices.Authentication.Services.IAuthenticationService;

namespace DotLogix.WebServices.Authentication.Extensions; 

public static class ServiceProviderExtensions {
    public static AuthenticationBuilder AddAuthentication<TService>(this IServiceCollection services, string schemeName, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TService : class, IAuthenticationService {
        return AddAuthentication<IAuthenticationService, TService>(services, schemeName, lifetime);
    }
        
    public static AuthenticationBuilder AddAuthentication<TService, TImplementation>(this IServiceCollection services, string schemeName, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TService : class, IAuthenticationService
        where TImplementation : class, TService {
        services.Add(typeof(TService), typeof(TImplementation), lifetime);
        return AddAuthenticationCore<TService>(services, schemeName);
    }

    private static AuthenticationBuilder AddAuthenticationCore<TService>(IServiceCollection services, string schemeName)
        where TService : class, IAuthenticationService {
        var authenticationBuilder = services
           .AddAuthentication()
           .AddScheme<AuthenticationSchemeOptions, AuthenticationServiceHandler<TService>>(schemeName, default);
        return authenticationBuilder;
    }
}