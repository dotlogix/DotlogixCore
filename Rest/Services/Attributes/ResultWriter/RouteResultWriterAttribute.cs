// ==================================================
// Copyright 2018(C) , DotLogix
// File:  RouteResultWriterAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.06.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Attributes;
using DotLogix.Core.Rest.Services.Writer;
using DotLogix.Core.Utils;
#endregion

namespace DotLogix.Core.Rest.Services.Attributes.ResultWriter {
    [AttributeUsage(AttributeTargets.Method)]
    public class RouteResultWriterAttribute : InstantiatorAttribute {
        public RouteResultWriterAttribute(Type singletonType, string propertyName) : base(singletonType, propertyName, typeof(IAsyncWebRequestResultWriter)) { }
        public RouteResultWriterAttribute(Type type) : base(type, typeof(IAsyncWebRequestResultWriter)) { }
        protected RouteResultWriterAttribute(IInstantiator instantiator) : base(instantiator) { }
        protected RouteResultWriterAttribute(Func<object> instantiateFunc) : base(instantiateFunc) { }

        public IAsyncWebRequestResultWriter CreateResultWriter() {
            return GetInstance<IAsyncWebRequestResultWriter>();
        }
    }
}
