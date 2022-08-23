using System;
using System.Collections.Generic;
using System.Net;
using DotLogix.Core.Extensions;

namespace DotLogix.WebServices.Core.Errors; 

public abstract class TypedApiException : ApiException {
    public Type ClrType { get; set; }

    public TypedApiException(string kind, string message = null, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        : base(kind, message, statusCode) {
    }
        
    public TypedApiException(string kind, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        : this(kind, null, statusCode) {
    }

    protected override void AppendContext(IDictionary<string, object> dictionary) {
        if(ClrType is not null) {
            dictionary.Add("Type", ClrType.GetFriendlyName());
        }
    }
}