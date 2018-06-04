using System;
using DotLogix.Core.Extensions;
using DotLogix.Core.Rest.Services.Writer;

namespace DotLogix.Core.Rest.Services.Attributes.ResultWriter {
    public class DynamicRouteResultWriterAttribute : RouteResultWriterAttribute {
        public Type RequestResultWriterType { get; }
        public DynamicRouteResultWriterAttribute(Type requestResultWriterType) {
            if(requestResultWriterType.IsAssignableTo(typeof(IAsyncWebRequestResultWriter)) == false)
                throw new ArgumentException($"Type {requestResultWriterType.GetFriendlyName()} is not assignable to {nameof(IAsyncWebRequestResultWriter)}", nameof(requestResultWriterType));

            this.RequestResultWriterType = requestResultWriterType;
        }

        public override IAsyncWebRequestResultWriter CreateResultWriter() {
            return RequestResultWriterType?.Instantiate<IAsyncWebRequestResultWriter>();
        }
    }
}