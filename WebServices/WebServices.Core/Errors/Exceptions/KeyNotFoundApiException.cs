using System.Collections.Generic;
using System.Net;

namespace DotLogix.WebServices.Core.Errors; 

public class KeyNotFoundApiException : TypedApiException {
    public object Key { get; set; }

    public KeyNotFoundApiException(object key = null)
        : base(ApiErrorKinds.KeyNotFound, HttpStatusCode.NotFound) {
        Key = key;
    }

    protected override string GetErrorMessage() {
        return $"Can not find object of type {ClrType} matching key {Key}";
    }

    protected override void AppendContext(IDictionary<string, object> dictionary) {
        base.AppendContext(dictionary);
        if(Key is not null) {
            dictionary.Add("Key", Key.ToString());
        }
    }
}