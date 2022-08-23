// ==================================================
// Copyright 2018(C) , DotLogix
// File:  FluentIlExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Reflection;
using System.Reflection.Emit;
using DotLogix.Core.Reflection.Fluent.Generator;
#endregion

namespace DotLogix.Core.Reflection.Fluent; 

/// <summary>
/// A static class providing extension methods for reflection
/// </summary>
public static class FluentIlExtensions {
    /// <summary>
    /// Create a fluent implementation of an il generator
    /// </summary>
    public static FluentIlGenerator AsFluent(this ILGenerator ilGenerator) {
        return new FluentIlGenerator(ilGenerator);
    }

    /// <summary>
    /// Create a fluent implementation of an il generator
    /// </summary>
    public static FluentIlGenerator GetFluentIlGenerator(this DynamicMethod dynamicMethod) {
        return new FluentIlGenerator(dynamicMethod.GetILGenerator());
    }

    /// <summary>
    /// Create a fluent implementation of an il generator
    /// </summary>
    public static FluentIlGenerator GetFluentIlGenerator(this ConstructorBuilder constructorBuilder) {
        return new FluentIlGenerator(constructorBuilder.GetILGenerator());
    }

    /// <summary>
    /// Create a fluent implementation of an il generator
    /// </summary>
    public static FluentIlGenerator GetFluentIlGenerator(this MethodBuilder methodBuilder) {
        return new FluentIlGenerator(methodBuilder.GetILGenerator());
    }

    /// <summary>
    /// Create a fluent implementation of an il generator
    /// </summary>
    public static FluentIlGenerator GetFluentIlGenerator(this DynamicMethod dynamicMethod, int streamSize) {
        return new FluentIlGenerator(dynamicMethod.GetILGenerator(streamSize));
    }

    /// <summary>
    /// Create a fluent implementation of an il generator
    /// </summary>
    public static FluentIlGenerator GetFluentIlGenerator(this ConstructorBuilder constructorBuilder, int streamSize) {
        return new FluentIlGenerator(constructorBuilder.GetILGenerator(streamSize));
    }

    /// <summary>
    /// Create a fluent implementation of an il generator
    /// </summary>
    public static FluentIlGenerator GetFluentIlGenerator(this MethodBuilder methodBuilder, int streamSize) {
        return new FluentIlGenerator(methodBuilder.GetILGenerator(streamSize));
    }

    /// <summary>
    /// Checks if a property is an indexer
    /// </summary>
    public static bool IsIndexerProperty(this PropertyInfo propertyInfo) {
        return propertyInfo.GetIndexParameters().Length > 0;
    }
}