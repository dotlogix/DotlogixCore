using System;
using System.Reflection;
using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Rest.Services.Descriptors;

namespace DotLogix.Core.Rest.Services.Attributes.Descriptors {
    public class SingletonDescriptorAttribute : DescriptorAttribute
    {
        private readonly DynamicProperty _property;

        public SingletonDescriptorAttribute(Type singletonHolderType, string propertyName="Instance") {
            _property = singletonHolderType.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Static)?.CreateDynamicProperty();
            if(_property == null)
                throw new InvalidOperationException($"Type {singletonHolderType.GetFriendlyName()} does not have a static property with the name {propertyName}");

            if(_property.ValueType.IsAssignableTo(typeof(IWebRequestProcessorDescriptor)))
                throw new InvalidOperationException($"The type of the property {singletonHolderType.GetFriendlyName()}.{propertyName} is not assignable to {nameof(IWebRequestProcessorDescriptor)}");
        }

        public override IWebRequestProcessorDescriptor CreateDescriptor() {
            return _property.GetValue() as IWebRequestProcessorDescriptor;
        }
    }
}