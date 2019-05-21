// ==================================================
// Copyright 2018(C) , DotLogix
// File:  InstantiatorAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.08.2018
// LastEdited:  13.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Utils;
#endregion

namespace DotLogix.Core.Attributes {
    public class InstantiatorAttribute : Attribute {
        private readonly IInstantiator _instantiator;

        public InstantiatorAttribute(IInstantiator instantiator) {
            _instantiator = instantiator ?? throw new ArgumentNullException(nameof(instantiator));
        }

        public InstantiatorAttribute(Type singletonType, string propertyName, Type constraintType = null) : this(Instantiator.UseSingletonProperty(singletonType, propertyName, constraintType)) { }

        protected InstantiatorAttribute(Type type, Type constraintType = null) : this(Instantiator.UseDefaultCtor(type, constraintType)) { }

        protected InstantiatorAttribute(Func<object> instantiateFunc) : this(Instantiator.UseDelegate(instantiateFunc)) { }

        protected object GetInstance() {
            return _instantiator.GetInstance();
        }

        protected TInstance GetInstance<TInstance>() {
            return _instantiator.GetInstance() is TInstance instance ? instance : default;
        }
    }
}
