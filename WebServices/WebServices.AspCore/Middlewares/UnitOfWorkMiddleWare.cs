#region using
using System.Net;
using System.Threading.Tasks;
using DotLogix.Core.Diagnostics;
using DotLogix.Infrastructure;
using DotLogix.WebServices.AspCore.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
#endregion

namespace DotLogix.WebServices.AspCore.Middlewares
{
    public class UnitOfWorkMiddleware : IMiddleware
    {
        private ILogSource<UnitOfWorkMiddleware> LogSource { get; }

        public UnitOfWorkMiddleware(ILogSource<UnitOfWorkMiddleware> logSource) {
            LogSource = logSource;
        }

        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            var serviceProvider = httpContext.RequestServices;
            var entityContext = serviceProvider.GetRequiredService<IEntityContext>();

            // https://github.com/aws/aws-lambda-dotnet/issues/570
            httpContext.Response.StatusCode = (int)HttpStatusCode.OK;

            await next.Invoke(httpContext).ConfigureAwait(false);

            var statusCode = (HttpStatusCode)httpContext.Response.StatusCode;
            if(statusCode.IndicatesSuccess() || statusCode.IndicatesRedirect()) {
                var affectedEntities = await entityContext.CompleteAsync();
                LogSource.Debug($"Committed {affectedEntities} changes to the database");
            } else {
                LogSource.Warn($"Response status code {(int)statusCode} does not indicate success, discarded possible db changes");
            }
        }

    }
}
