#region using
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DotLogix.Core.Diagnostics;
using DotLogix.WebServices.Core.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
#endregion

namespace DotLogix.WebServices.AspCore.Middlewares
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogSource _logSource;

        public ErrorHandlingMiddleware(ILogSource<ErrorHandlingMiddleware> logSource)
        {
            _logSource = logSource;
        }

        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            var originBody = httpContext.Response.Body;
            try
            {
                var memStream = new MemoryStream();
                httpContext.Response.Body = memStream;

                // we have to do this because the status code otherwise shows up as 0 in the lambda
                // https://github.com/aws/aws-lambda-dotnet/issues/570
                httpContext.Response.StatusCode = (int) HttpStatusCode.OK;

                await next.Invoke(httpContext).ConfigureAwait(false);

                memStream.Seek(0, SeekOrigin.Begin);
                await memStream.CopyToAsync(originBody);
            }
            catch (Exception e)
            {
                await OnRequestExceptionAsync(httpContext, e, originBody);
            }
            finally
            {
                httpContext.Response.Body = originBody;
            }
        }

        protected virtual async Task OnRequestExceptionAsync(HttpContext httpContext, Exception exception, Stream originalBody)
        {
            _logSource.Error($"Response does not indicate success, discarded possible db changes:\n{exception}");
            if (httpContext.Response.HasStarted)
            {
                return;
            }

            var apiError = GetApiError(exception);

#if !DEBUG
            apiError.Context = null;
#endif
            var json = JsonConvert.SerializeObject(apiError);
            var bytes = Encoding.UTF8.GetBytes(json);

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int) apiError.StatusCode;
            httpContext.Response.ContentLength = bytes.Length;

            await originalBody.WriteAsync(bytes, 0, bytes.Length);
        }

        protected virtual ApiError GetApiError(Exception exception)
        {
            return GetExceptionApiError(exception)
                ?? GetDatabaseApiError(exception)
                ?? GetDefaultApiError(exception);
        }
        protected ApiError GetExceptionApiError(Exception exception) {
            var apiException = GetExceptionRecursive<ApiException>(exception);
            return apiException?.GetApiError();
        }
        protected ApiError GetDatabaseApiError(Exception exception) {
            var dbException = GetExceptionRecursive<DbUpdateException>(exception);
            if(dbException == null) {
                return null;
            }

            var entityEntry = dbException.Entries.First();
            return new ApiError {
                Kind = HttpStatusCode.Conflict.ToString(),
                Message = dbException.ToString(),
                StatusCode = HttpStatusCode.Conflict,
                Context = new Dictionary<string, object> {
                    {"CurrentValues", entityEntry.CurrentValues},
                    {"OriginalValues", entityEntry.OriginalValues}
                }
            };
        }
        protected ApiError GetDefaultApiError(Exception exception) {
            return new ApiError
            {
                Kind = HttpStatusCode.InternalServerError.ToString(),
                Message = exception.ToString(),
                StatusCode = HttpStatusCode.InternalServerError
            };
        }

        protected virtual TException GetExceptionRecursive<TException>(Exception exception) where TException : Exception
        {
            if (exception == null)
            {
                return null;
            }

            switch (exception)
            {
                case AggregateException aggregateException:
                    foreach (var innerException in aggregateException.InnerExceptions)
                    {
                        var matchingException = GetExceptionRecursive<TException>(innerException);
                        if (matchingException != null)
                        {
                            return matchingException;
                        }
                    }

                    break;
                case TException matchingException:
                    return matchingException;
            }

            return exception.InnerException != null
                ? GetExceptionRecursive<TException>(exception.InnerException)
                : default;
        }
    }
}