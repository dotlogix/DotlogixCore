#region using
using System;
using DotLogix.Core.Diagnostics;
using DotLogix.WebServices.Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
#endregion

namespace DotLogix.WebServices.AspCore {
    public static class ServiceProviders {
        private static Func<IServiceProvider> _getRequestServices;

        public static IServiceProvider ApplicationServices { get; private set; }
        public static IServiceProvider RequestServices => _getRequestServices.Invoke();
        public static HttpContext HttpContext => GetApplicationService<IHttpContextAccessor>()?.HttpContext;
        
        public static ILogger Logger => GetApplicationService<ILogger>();
        public static ILogSourceProvider LogSourceProvider => GetApplicationService<ILogSourceProvider>();
        
        public static T GetService<T>(bool required = false) where T : class => (RequestServices ?? ApplicationServices)?.GetService<T>(required);
        public static T GetRequestService<T>(bool required = false) where T : class => RequestServices?.GetService<T>(required);
        public static T GetApplicationService<T>(bool required = false) where T : class => ApplicationServices?.GetService<T>(required);
        
        
        public static void UseStaticServiceProviders(this IApplicationBuilder builder) {
            UseStaticServiceProviders(builder.ApplicationServices, () => HttpContext?.RequestServices);
        }
        
        public static void UseStaticServiceProviders(IServiceProvider applicationServices, Func<IServiceProvider> getRequestServices) {
            ApplicationServices = applicationServices;
            _getRequestServices = getRequestServices;
        }
    }
}
