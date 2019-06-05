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
    /// An attribute to create an instance of <see cref="Instantiator"/>
    /// </summary>
    public class InstantiatorAttribute : Attribute {
        private readonly IInstantiator _instantiator;

        /// <summary>
        /// Creates an instance of <see cref="InstantiatorAttribute"/> using an existing <see cref="IInstantiator"/>
        /// </summary>
        /// <param name="instantiator">The instance</param>
        public InstantiatorAttribute(IInstantiator instantiator) {
            _instantiator = instantiator ?? throw new ArgumentNullException(nameof(instantiator));
        }

        /// <summary>
        /// Creates an instance of <see cref="InstantiatorAttribute"/> using a static property of another type
        /// </summary>
        /// <param name="singletonType">The type containing the property</param>
        /// <param name="propertyName">The property name</param>
        /// <param name="constraintType">An optional constraint type to ensure type compatibility</param>
        public InstantiatorAttribute(Type singletonType, string propertyName, Type constraintType = null) : this(Instantiator.UseSingletonProperty(singletonType, propertyName, constraintType)) { }

        /// <summary>
        /// Creates an instance of <see cref="InstantiatorAttribute"/> using the default constructor of another type
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="constraintType">An optional constraint type to ensure type compatibility</param>
        protected InstantiatorAttribute(Type type, Type constraintType = null) : this(Instantiator.UseDefaultCtor(type, constraintType)) { }

        /// <summary>
        /// Creates an instance of <see cref="InstantiatorAttribute"/> using a delegate method
        /// </summary>
        /// <param name="instantiateFunc">The factory method</param>
        protected InstantiatorAttribute(Func<object> instantiateFunc) : this(Instantiator.UseDelegate(instantiateFunc)) { }

        /// <summary>
        /// Get or create an instance using the configured method
        /// </summary>
        /// <returns></returns>
        protected object GetInstance() {
            return _instantiator.GetInstance();
        }

        /// <summary>
        /// Get or create an instance as a specific type using the configured method
        /// </summary>
        /// <typeparam name="TInstance">The target type</typeparam>
        /// <returns>The instance if the provided type is compatible, otherwise <value>default</value></returns>
        protected TInstance GetInstance<TInstance>() {
            return _instantiator.GetInstance() is TInstance instance ? instance : default;
        }
    }

    /// <summary>
    /// An attribute to create an instance of <see cref="IArgsInstantiator"/>
    /// </summary>
    public class ArgsInstantiatorAttribute : Attribute
    {
        private readonly IArgsInstantiator _instantiator;

        /// <summary>
        /// Creates an instance of <see cref="ArgsInstantiatorAttribute"/> using an existing <see cref="IInstantiator"/>
        /// </summary>
        /// <param name="instantiator">The instance</param>
        public ArgsInstantiatorAttribute(IArgsInstantiator instantiator)
        {
            _instantiator = instantiator ?? throw new ArgumentNullException(nameof(instantiator));
        }

        /// <summary>
        /// Creates an instance of <see cref="InstantiatorAttribute"/> using a constructor with the specified type arguments
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="parameterTypes">The parameter types used by the constructor</param>
        /// <param name="constraintType">An optional constraint type to ensure type compatibility</param>
        protected ArgsInstantiatorAttribute(Type type, Type[] parameterTypes, Type constraintType = null) : this(Instantiator.UseCtor(type, parameterTypes, constraintType)) { }

        /// <summary>
        /// Creates an instance of <see cref="InstantiatorAttribute"/> using a delegate method
        /// </summary>
        /// <param name="instantiateFunc">The factory method</param>
        protected ArgsInstantiatorAttribute(Func<object[], object> instantiateFunc) : this(Instantiator.UseDelegate(instantiateFunc)) { }

        /// <summary>
        /// Get or create an instance using the configured method
        /// </summary>
        /// <param name="args">The arguments to create an instance</param>
        /// <returns></returns>
        protected object GetInstance(params object[] args)
        {
            return _instantiator.GetInstance(args);
        }

        /// <summary>
        /// Get or create an instance as a specific type using the configured method
        /// </summary>
        /// <param name="args">The arguments to create an instance</param>
        /// <typeparam name="TInstance">The target type</typeparam>
        /// <returns>The instance if the provided type is compatible, otherwise <value>default</value></returns>
        protected TInstance GetInstance<TInstance>(params object[] args)
        {
            return _instantiator.GetInstance(args) is TInstance instance ? instance : default;
        }
    }
}
