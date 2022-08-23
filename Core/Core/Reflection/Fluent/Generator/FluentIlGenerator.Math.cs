// ==================================================
// Copyright 2018(C) , DotLogix
// File:  FluentIlGenerator.Math.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Reflection.Emit;
#endregion

namespace DotLogix.Core.Reflection.Fluent.Generator; 

public partial class FluentIlGenerator {
    /// <summary>
    ///     <inheritdoc cref="OpCodes.Shl"/>
    /// </summary>
    public FluentIlGenerator Shl() {
        IlGenerator.Emit(OpCodes.Shl);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Shr"/>
    /// </summary>
    public FluentIlGenerator Shr() {
        IlGenerator.Emit(OpCodes.Shr);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Shr_Un"/>
    /// </summary>
    public FluentIlGenerator Shr_Un() {
        IlGenerator.Emit(OpCodes.Shr_Un);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Rem"/>
    /// </summary>
    public FluentIlGenerator Rem() {
        IlGenerator.Emit(OpCodes.Rem);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Rem_Un"/>
    /// </summary>
    public FluentIlGenerator Rem_Un() {
        IlGenerator.Emit(OpCodes.Rem_Un);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Sub"/>
    /// </summary>
    public FluentIlGenerator Sub() {
        IlGenerator.Emit(OpCodes.Sub);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Sub_Ovf"/>
    /// </summary>
    public FluentIlGenerator Sub_Ovf() {
        IlGenerator.Emit(OpCodes.Sub_Ovf);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Sub_Ovf_Un"/>
    /// </summary>
    public FluentIlGenerator Sub_Ovf_Un() {
        IlGenerator.Emit(OpCodes.Sub_Ovf_Un);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Add"/>
    /// </summary>
    public FluentIlGenerator Add() {
        IlGenerator.Emit(OpCodes.Add);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Add_Ovf"/>
    /// </summary>
    public FluentIlGenerator Add_Ovf() {
        IlGenerator.Emit(OpCodes.Add_Ovf);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Add_Ovf_Un"/>
    /// </summary>
    public FluentIlGenerator Add_Ovf_Un() {
        IlGenerator.Emit(OpCodes.Add_Ovf_Un);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Div"/>
    /// </summary>
    public FluentIlGenerator Div() {
        IlGenerator.Emit(OpCodes.Div);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Div_Un"/>
    /// </summary>
    public FluentIlGenerator Div_Un() {
        IlGenerator.Emit(OpCodes.Div_Un);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Mul"/>
    /// </summary>
    public FluentIlGenerator Mul() {
        IlGenerator.Emit(OpCodes.Mul);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Mul_Ovf"/>
    /// </summary>
    public FluentIlGenerator Mul_Ovf() {
        IlGenerator.Emit(OpCodes.Mul_Ovf);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Mul_Ovf_Un"/>
    /// </summary>
    public FluentIlGenerator Mul_Ovf_Un() {
        IlGenerator.Emit(OpCodes.Mul_Ovf_Un);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Neg"/>
    /// </summary>
    public FluentIlGenerator Neg() {
        IlGenerator.Emit(OpCodes.Neg);
        return this;
    }
}