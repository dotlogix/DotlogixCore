using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

namespace DotLogix.WebServices.Core.Errors; 

public abstract class ApiExceptionBase : Exception {
    public string Kind { get; }
    public HttpStatusCode StatusCode { get; }
    [Browsable(false)]
    public sealed override string Message => GetErrorMessage();
    public IReadOnlyDictionary<string, object> Context {
        get {
            var dict = new SortedDictionary<string, object>();
            AppendContext(dict);
            return dict.Count > 0 ? dict : null;
        }
    }

    protected ApiExceptionBase(string kind, HttpStatusCode statusCode) {
        Kind = kind;
        StatusCode = statusCode;
    }

    protected virtual string GetErrorMessage() => null;
    protected virtual void AppendContext(IDictionary<string, object> dictionary) { }

    public ApiError GetApiError() {
        return new ApiError {
            Kind = Kind,
            StatusCode = StatusCode,
            Message = Message,
            Context = Context
        };
    }
}