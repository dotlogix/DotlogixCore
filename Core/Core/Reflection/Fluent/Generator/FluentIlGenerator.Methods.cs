// ==================================================
// Copyright 2018(C) , DotLogix
// File:  FluentIlGenerator.Methods.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Reflection;
using System.Reflection.Emit;
#endregion

namespace DotLogix.Core.Reflection.Fluent.Generator; 

public partial class FluentIlGenerator {
    #region Values
    /// <summary>
    ///     <inheritdoc cref="OpCodes.Constrained"/>
    /// </summary>
    public FluentIlGenerator Constrained(Type type) {
        IlGenerator.Emit(OpCodes.Constrained, type);
        return this;
    }
    #endregion

    #region Methods
    #region Call
    /// <summary>
    ///     <inheritdoc cref="OpCodes.Call"/>
    /// </summary>
    public FluentIlGenerator Call(MethodInfo methodInfo) {
        IlGenerator.Emit(OpCodes.Call, methodInfo);
        return this;
    }
        
    /// <summary>
    ///     <inheritdoc cref="OpCodes.Call"/>
    /// </summary>
    public FluentIlGenerator Call(ConstructorInfo constructorInfo) {
        IlGenerator.Emit(OpCodes.Call, constructorInfo);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Call"/>
    /// </summary>
    public FluentIlGenerator Call(MethodInfo methodInfo, Type[] optionalParameterTypes = null) {
        IlGenerator.EmitCall(OpCodes.Call, methodInfo, optionalParameterTypes);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Calli"/>
    /// </summary>
    public FluentIlGenerator Calli(CallingConventions callingConvention, Type returnType, Type[] parameterTypes,
        Type[] optionalParameterTypes) {
        IlGenerator.EmitCalli(OpCodes.Calli, callingConvention, returnType, parameterTypes, optionalParameterTypes);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Callvirt"/>
    /// </summary>
    public FluentIlGenerator Callvirt(MethodInfo methodInfo) {
        IlGenerator.Emit(OpCodes.Call, methodInfo);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Callvirt"/>
    /// </summary>
    public FluentIlGenerator Callvirt(MethodInfo methodInfo, Type[] optionalParameterTypes) {
        IlGenerator.EmitCall(OpCodes.Call, methodInfo, optionalParameterTypes);
        return this;
    }
    #endregion

    #region Load Arg
    /// <summary>
    ///     <inheritdoc cref="OpCodes.Ldarg"/>
    /// </summary>
    public FluentIlGenerator Ldarg(short index) {
        IlGenerator.Emit(OpCodes.Ldarg, index);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Ldarg_0"/>
    /// </summary>
    public FluentIlGenerator Ldarg_0() {
        IlGenerator.Emit(OpCodes.Ldarg_0);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Ldarg_1"/>
    /// </summary>
    public FluentIlGenerator Ldarg_1() {
        IlGenerator.Emit(OpCodes.Ldarg_1);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Ldarg_2"/>
    /// </summary>
    public FluentIlGenerator Ldarg_2() {
        IlGenerator.Emit(OpCodes.Ldarg_2);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Ldarg_3"/>
    /// </summary>
    public FluentIlGenerator Ldarg_3() {
        IlGenerator.Emit(OpCodes.Ldarg_3);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Ldarg_S"/>
    /// </summary>
    public FluentIlGenerator Ldarg_S(byte index) {
        IlGenerator.Emit(OpCodes.Ldarg_S, index);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Ldarga"/>
    /// </summary>
    public FluentIlGenerator Ldarga(short index) {
        IlGenerator.Emit(OpCodes.Ldarga, index);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Ldarga_S"/>
    /// </summary>
    public FluentIlGenerator Ldarga_S(byte index) {
        IlGenerator.Emit(OpCodes.Ldarga_S, index);
        return this;
    }
    #endregion

    #region Store Arg
    /// <summary>
    ///     <inheritdoc cref="OpCodes.Starg_S"/>
    /// </summary>
    public FluentIlGenerator Starg_S(byte index) {
        IlGenerator.Emit(OpCodes.Starg_S, index);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Starg"/>
    /// </summary>
    public FluentIlGenerator Starg(short index) {
        IlGenerator.Emit(OpCodes.Starg, index);
        return this;
    }
    #endregion

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Arglist"/>
    /// </summary>
    public FluentIlGenerator Arglist() {
        IlGenerator.Emit(OpCodes.Arglist);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Ret"/>
    /// </summary>
    public FluentIlGenerator Ret() {
        IlGenerator.Emit(OpCodes.Ret);
        return this;
    }
    #endregion
}