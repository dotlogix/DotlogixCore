using System;
using DotLogix.Core.Rest.Services.Writer;

namespace DotLogix.Core.Rest.Services.Attributes.ResultWriter {
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class RouteResultWriterAttribute : Attribute {
        public abstract IAsyncWebRequestResultWriter CreateResultWriter();
    }
}