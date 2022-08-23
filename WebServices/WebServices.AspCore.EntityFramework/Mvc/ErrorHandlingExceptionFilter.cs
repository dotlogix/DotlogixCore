using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DotLogix.Core.Diagnostics;
using DotLogix.WebServices.Core.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace DotLogix.WebServices.AspCore.Mvc; 

public class ErrorHandlingExceptionFilter : IExceptionFilter
{
    protected ILogSource LogSource { get; }

    protected ErrorHandlingExceptionFilter(ILogSource logSource) {
        LogSource = logSource;
    }
    public ErrorHandlingExceptionFilter(ILogSource<ErrorHandlingExceptionFilter> logSource) {
        LogSource = logSource;
    }

    public void OnException(ExceptionContext context)
    {
        if (context.ExceptionHandled)
        {
            return;
        }

        var exception = context.Exception;
        var apiError = GetApiError(exception);
        var statusCode = apiError.StatusCode;
        LogSource.Error(exception, apiError.Context);

#if !DEBUG
            apiError.Message = "Internal Server Error";
#endif

        context.Result = new ObjectResult(apiError) { StatusCode = (int) statusCode };
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

        var conflictEntries = dbException.Entries
           .Select(entityEntry => new Dictionary<string, object> {
                { "original", entityEntry.OriginalValues },
                { "conflict", entityEntry.CurrentValues }
            }).ToList();
            
        return new ApiError {
            Kind = HttpStatusCode.Conflict.ToString(),
            Message = dbException.ToString(),
            StatusCode = HttpStatusCode.Conflict,
            Context = new Dictionary<string, object> {
                { "values", conflictEntries }
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
                    if (matchingException is not null)
                    {
                        return matchingException;
                    }
                }

                break;
            case TException matchingException:
                return matchingException;
        }

        return exception.InnerException is not null
            ? GetExceptionRecursive<TException>(exception.InnerException)
            : default;
    }
}