// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeConverterAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.08.2018
// LastEdited:  13.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Attributes;
using DotLogix.Core.Nodes.Converters;
using DotLogix.Core.Utils;
#endregion

namespace DotLogix.Core.Nodes {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class NodeConverterAttribute : InstantiatorAttribute {
        public NodeConverterAttribute(Type singletonType, string propertyName) : base(singletonType, propertyName, typeof(IAsyncNodeConverter)) { }
        public NodeConverterAttribute(Type type) : base(type, typeof(IAsyncNodeConverter)) { }
        protected NodeConverterAttribute(Func<object> instantiateFunc) : base(instantiateFunc) { }
        protected NodeConverterAttribute(IInstantiator instantiator) : base(instantiator) { }

        public IAsyncNodeConverter CreateNodeConverter() {
            return GetInstance<IAsyncNodeConverter>();
        }
    }
}
