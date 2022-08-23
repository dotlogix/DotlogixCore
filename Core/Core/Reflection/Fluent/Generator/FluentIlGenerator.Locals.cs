// ==================================================
// Copyright 2018(C) , DotLogix
// File:  FluentIlGenerator.Locals.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Reflection.Emit;
#endregion

namespace DotLogix.Core.Reflection.Fluent.Generator; 

public partial class FluentIlGenerator {
    #region Load
    /// <summary>
    ///     <inheritdoc cref="OpCodes.Ldloc"/>
    /// </summary>
    public FluentIlGenerator Ldloc(short index) {
        IlGenerator.Emit(OpCodes.Ldloc, index);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Ldloc"/>
    /// </summary>
    public FluentIlGenerator Ldloc(LocalBuilder localBuilder) {
        IlGenerator.Emit(OpCodes.Ldloc, localBuilder);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Ldloc_0"/>
    /// </summary>
    public FluentIlGenerator Ldloc_0() {
        IlGenerator.Emit(OpCodes.Ldloc_0);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Ldloc_1"/>
    /// </summary>
    public FluentIlGenerator Ldloc_1() {
        IlGenerator.Emit(OpCodes.Ldloc_1);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Ldloc_2"/>
    /// </summary>
    public FluentIlGenerator Ldloc_2() {
        IlGenerator.Emit(OpCodes.Ldloc_2);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Ldloc_3"/>
    /// </summary>
    public FluentIlGenerator Ldloc_3() {
        IlGenerator.Emit(OpCodes.Ldloc_3);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Ldloc_S"/>
    /// </summary>
    public FluentIlGenerator Ldloc_S(byte index) {
        IlGenerator.Emit(OpCodes.Ldloc_S, index);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Ldloc_S"/>
    /// </summary>
    public FluentIlGenerator Ldloc_S(LocalBuilder localBuilder) {
        IlGenerator.Emit(OpCodes.Ldloc_S, localBuilder);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Ldloca"/>
    /// </summary>
    public FluentIlGenerator Ldloca(short index) {
        IlGenerator.Emit(OpCodes.Ldloca, index);
        return this;
    }

        
    /// <summary>
    ///     <inheritdoc cref="OpCodes.Ldloca"/>
    /// </summary>
    public FluentIlGenerator Ldloca(LocalBuilder localBuilder) {
        IlGenerator.Emit(OpCodes.Ldloca, localBuilder);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Ldloca_S"/>
    /// </summary>
    public FluentIlGenerator Ldloca_S(byte index) {
        IlGenerator.Emit(OpCodes.Ldloca_S, index);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Ldloca_S"/>
    /// </summary>
    public FluentIlGenerator Ldloca_S(LocalBuilder localBuilder) {
        IlGenerator.Emit(OpCodes.Ldloca_S, localBuilder);
        return this;
    }
    #endregion

    #region Store
    /// <summary>
    ///     <inheritdoc cref="OpCodes.Stloc"/>
    /// </summary>
    public FluentIlGenerator Stloc(short index) {
        IlGenerator.Emit(OpCodes.Stloc, index);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Stloc"/>
    /// </summary>
    public FluentIlGenerator Stloc(LocalBuilder localBuilder) {
        IlGenerator.Emit(OpCodes.Stloc, localBuilder);
        return this;
    }

    /// <summary>
    ///     Pops the current value from the top of the evaluation stack and stores it in a the local variable list at
    ///     index 0.
    /// </summary>
    public FluentIlGenerator Stloc_0() {
        IlGenerator.Emit(OpCodes.Stloc_0);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Stloc_1"/>
    /// </summary>
    public FluentIlGenerator Stloc_1() {
        IlGenerator.Emit(OpCodes.Stloc_1);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Stloc_2"/>
    /// </summary>
    public FluentIlGenerator Stloc_2() {
        IlGenerator.Emit(OpCodes.Stloc_2);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Stloc_3"/>
    /// </summary>
    public FluentIlGenerator Stloc_3() {
        IlGenerator.Emit(OpCodes.Stloc_3);
        return this;
    }

        
    /// <summary>
    ///     <inheritdoc cref="OpCodes.Stloc_S"/>
    /// </summary>
    public FluentIlGenerator Stloc_S(byte index) {
        IlGenerator.Emit(OpCodes.Stloc_S, index);
        return this;
    }

        
    /// <summary>
    ///     <inheritdoc cref="OpCodes.Stloc_S"/>
    /// </summary>
    public FluentIlGenerator Stloc_S(LocalBuilder localBuilder) {
        IlGenerator.Emit(OpCodes.Stloc_S, localBuilder);
        return this;
    }
    #endregion
}