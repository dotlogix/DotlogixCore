using System;
using DotLogix.Core.Rest.Server.Http;

namespace DotLogix.Core.Rest.Services.Attributes.Http {
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpDeleteAttribute : HttpMethodAttribute {
        public HttpDeleteAttribute() : base(HttpMethods.Delete) { }
    }
}