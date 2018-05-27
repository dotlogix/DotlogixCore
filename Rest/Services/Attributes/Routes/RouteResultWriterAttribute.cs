using System;
using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Services.Writer;

namespace DotLogix.Core.Rest.Services.Attributes.Routes {
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class RouteResultWriterAttribute : Attribute {
        public abstract IAsyncWebRequestResultWriter CreateResultWriter();
    }
}