using System;
using System.Reflection;
using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Rest.Services.Writer;

namespace DotLogix.Core.Rest.Services.Attributes.ResultWriter {
    public class SingletonRouteResultWriterAttribute : RouteResultWriterAttribute {
        private readonly DynamicProperty _property;

        public SingletonRouteResultWriterAttribute(Type singletonHolderType, string propertyName="Instance") {
            _property = singletonHolderType.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Static)?.CreateDynamicProperty();
            if(_property == null)
                throw new InvalidOperationException($"Type {singletonHolderType.GetFriendlyName()} does not have a static property with the name {propertyName}");

            if(_property.ValueType.IsAssignableTo(typeof(IAsyncWebRequestResultWriter)))
                throw new InvalidOperationException($"The type of the property {singletonHolderType.GetFriendlyName()}.{propertyName} is not assignable to {nameof(IAsyncWebRequestResultWriter)}");
        }

        public override IAsyncWebRequestResultWriter CreateResultWriter() {
            return _property.GetValue() as IAsyncWebRequestResultWriter;
        }
    }
}