// ==================================================
// Copyright 2018(C) , DotLogix
// File:  SingletonDescriptorAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.06.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Reflection;
using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Rest.Services.Attributes;
#endregion

namespace DotLogix.Core.Rest.Services.Descriptors {
    public class SingletonDescriptorAttribute : DescriptorAttribute {
        private readonly DynamicProperty _property;

        public SingletonDescriptorAttribute(Type singletonHolderType, string propertyName = "Instance") {
            _property = singletonHolderType.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Static)?.CreateDynamicProperty();
            if(_property == null)
                throw new InvalidOperationException($"Type {singletonHolderType.GetFriendlyName()} does not have a static property with the name {propertyName}");

            if(_property.ValueType.IsAssignableTo(typeof(IRouteDescriptor)))
                throw new InvalidOperationException($"The type of the property {singletonHolderType.GetFriendlyName()}.{propertyName} is not assignable to {nameof(IRouteDescriptor)}");
        }

        public override IRouteDescriptor CreateDescriptor() {
            return _property.GetValue() as IRouteDescriptor;
        }
    }
}
