#region using
using DotLogix.WebServices.AspCore.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
#endregion

namespace DotLogix.WebServices.AspCore.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void UseUnitOfWork(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<UnitOfWorkMiddleware>();
        }

        public static void AddUnitOfWork(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<UnitOfWorkMiddleware>();
        }

        public static void UseErrorHandling(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ErrorHandlingMiddleware>();
        }

        public static void AddErrorHandling(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ErrorHandlingMiddleware>();
        }
    }
}