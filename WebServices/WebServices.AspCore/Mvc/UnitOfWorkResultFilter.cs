using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using DotLogix.Core.Diagnostics;
using DotLogix.WebServices.AspCore.Extensions;
using DotLogix.WebServices.EntityFramework.Context;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DotLogix.WebServices.AspCore.Mvc
{
    [SuppressMessage("ReSharper", "EF1001")]
    public class UnitOfWorkResultFilter : IAsyncActionFilter
    {
        protected ILogSource LogSource { get; }

        protected UnitOfWorkResultFilter(ILogSource logSource)
        {
            LogSource = logSource;
        }

        public UnitOfWorkResultFilter(ILogSource<UnitOfWorkResultFilter> logSource)
        {
            LogSource = logSource;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var entityContext = httpContext.RequestServices.GetRequiredService<IEntityContext>();

            var result = await next.Invoke().ConfigureAwait(false);
            if (result.Canceled)
            {
                return;
            }

            if (result.Exception is not null)
            {
                LogSource.Warn("[Rollback] An error occured while processing the request, discarded changes to the database");
                return;
            }

            var statusCode = HttpStatusCode.OK;
            if (result.Result is IStatusCodeActionResult { StatusCode: { } } statusCodeResult)
            {
                statusCode = (HttpStatusCode)statusCodeResult.StatusCode.Value;
            }

            if (statusCode.IndicatesSuccess() || statusCode.IndicatesRedirect())
            {
                var affectedEntities = await entityContext.CompleteAsync();
                LogSource.Debug($"[Commit] Committed {affectedEntities} change(s) to the database");
            }
            else
            {
                LogSource.Warn($"[Rollback] Response status code {statusCode:D} {statusCode} does not indicate success, discarded changes to the database");
            }
        }
    }
}