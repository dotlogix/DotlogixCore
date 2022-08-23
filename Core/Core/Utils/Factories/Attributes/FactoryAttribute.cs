// ==================================================
// Copyright 2018(C) , DotLogix
// File:  InstantiatorAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.08.2018
// LastEdited:  13.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Utils.Factories; 

/// <summary>
/// An attribute to create a new instance of <see cref="Factories"/>
/// </summary>
public class FactoryAttribute : Attribute {
    private readonly IFactory _factory;

    /// <summary>
    /// Creates a new instance of <see cref="FactoryAttribute"/> using an existing <see cref="IFactory"/>
    /// </summary>
    /// <param name="factory">The instance</param>
    public FactoryAttribute(IFactory factory) {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    /// <summary>
    /// Creates a new instance of <see cref="FactoryAttribute"/> using a static property of another type
    /// </summary>
    /// <param name="singletonType">The type containing the property</param>
    /// <param name="propertyName">The property name</param>
    /// <param name="constraintType">An optional constraint type to ensure type compatibility</param>
    public FactoryAttribute(Type singletonType, string propertyName, Type constraintType = null) : this(Factories.UseSingletonProperty(singletonType, propertyName, constraintType)) { }

    /// <summary>
    /// Creates a new instance of <see cref="FactoryAttribute"/> using the default constructor of another type
    /// </summary>
    /// <param name="type">The type</param>
    /// <param name="constraintType">An optional constraint type to ensure type compatibility</param>
    protected FactoryAttribute(Type type, Type constraintType = null) : this(Factories.UseDefaultCtor(type, constraintType)) { }

    /// <summary>
    /// Creates a new instance of <see cref="FactoryAttribute"/> using a delegate method
    /// </summary>
    /// <param name="instantiateFunc">The factory method</param>
    protected FactoryAttribute(Func<object> instantiateFunc) : this(Factories.UseDelegate(instantiateFunc)) { }

    /// <summary>
    /// Get or create a new instance using the configured method
    /// </summary>
    /// <returns></returns>
    protected object GetInstance() {
        return _factory.Create();
    }

    /// <summary>
    /// Get or create a new instance as a specific type using the configured method
    /// </summary>
    /// <typeparam name="TInstance">The target type</typeparam>
    /// <returns>The instance if the provided type is compatible, otherwise <value>default</value></returns>
    protected TInstance GetInstance<TInstance>() {
        return _factory.Create() is TInstance instance ? instance : default;
    }
}