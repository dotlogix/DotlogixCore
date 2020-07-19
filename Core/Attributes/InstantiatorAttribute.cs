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
    /// <summary>
    /// An attribute to create a new instance of <see cref="Instantiator"/>
    /// </summary>
    public class InstantiatorAttribute : Attribute {
        private readonly IInstantiator _instantiator;

        /// <summary>
        /// Creates a new instance of <see cref="InstantiatorAttribute"/> using an existing <see cref="IInstantiator"/>
        /// </summary>
        /// <param name="instantiator">The instance</param>
        public InstantiatorAttribute(IInstantiator instantiator) {
            _instantiator = instantiator ?? throw new ArgumentNullException(nameof(instantiator));
        }

        /// <summary>
        /// Creates a new instance of <see cref="InstantiatorAttribute"/> using a static property of another type
        /// </summary>
        /// <param name="singletonType">The type containing the property</param>
        /// <param name="propertyName">The property name</param>
        /// <param name="constraintType">An optional constraint type to ensure type compatibility</param>
        public InstantiatorAttribute(Type singletonType, string propertyName, Type constraintType = null) : this(Instantiator.UseSingletonProperty(singletonType, propertyName, constraintType)) { }

        /// <summary>
        /// Creates a new instance of <see cref="InstantiatorAttribute"/> using the default constructor of another type
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="constraintType">An optional constraint type to ensure type compatibility</param>
        protected InstantiatorAttribute(Type type, Type constraintType = null) : this(Instantiator.UseDefaultCtor(type, constraintType)) { }

        /// <summary>
        /// Creates a new instance of <see cref="InstantiatorAttribute"/> using a delegate method
        /// </summary>
        /// <param name="instantiateFunc">The factory method</param>
        protected InstantiatorAttribute(Func<object> instantiateFunc) : this(Instantiator.UseDelegate(instantiateFunc)) { }

        /// <summary>
        /// Get or create a new instance using the configured method
        /// </summary>
        /// <returns></returns>
        protected object GetInstance() {
            return _instantiator.GetInstance();
        }

        /// <summary>
        /// Get or create a new instance as a specific type using the configured method
        /// </summary>
        /// <typeparam name="TInstance">The target type</typeparam>
        /// <returns>The instance if the provided type is compatible, otherwise <value>default</value></returns>
        protected TInstance GetInstance<TInstance>() {
            return _instantiator.GetInstance() is TInstance instance ? instance : default;
        }
    }
}
