// ==================================================
// Copyright 2018(C) , DotLogix
// File:  FluentIlGenerator.ControlTransfer.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Reflection;
using System.Reflection.Emit;
#endregion

namespace DotLogix.Core.Reflection.Fluent.Generator; 

public partial class FluentIlGenerator {
    /// <summary>
    ///     <inheritdoc cref="OpCodes.Jmp"/>
    /// </summary>
    public FluentIlGenerator Jmp(MethodInfo methodInfo) {
        IlGenerator.Emit(OpCodes.Jmp, methodInfo);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Switch"/>
    /// </summary>
    public FluentIlGenerator Switch(params Label[] labels) {
        IlGenerator.Emit(OpCodes.Switch, labels);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Endfilter"/>
    /// </summary>
    public FluentIlGenerator Endfilter() {
        IlGenerator.Emit(OpCodes.Endfilter);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Endfinally"/>
    /// </summary>
    public FluentIlGenerator Endfinally() {
        IlGenerator.Emit(OpCodes.Endfinally);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Leave"/>
    /// </summary>
    public FluentIlGenerator Leave(Label label) {
        IlGenerator.Emit(OpCodes.Leave, label);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Leave_S"/>
    /// </summary>
    public FluentIlGenerator Leave_S(Label label) {
        IlGenerator.Emit(OpCodes.Leave_S, label);
        return this;
    }

    #region Branch
    /// <summary>
    ///     <inheritdoc cref="OpCodes.Beq"/>
    /// </summary>
    public FluentIlGenerator Beq(Label label) {
        IlGenerator.Emit(OpCodes.Beq, label);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Beq_S"/>
    /// </summary>
    public FluentIlGenerator Beq_S(Label label) {
        IlGenerator.Emit(OpCodes.Beq_S, label);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Bge"/>
    /// </summary>
    public FluentIlGenerator Bge(Label label) {
        IlGenerator.Emit(OpCodes.Bge, label);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Bge_S"/>
    /// </summary>
    public FluentIlGenerator Bge_S(Label label) {
        IlGenerator.Emit(OpCodes.Bge_S, label);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Bge_Un"/>
    /// </summary>
    public FluentIlGenerator Bge_Un(Label label) {
        IlGenerator.Emit(OpCodes.Bge_Un, label);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Bge_Un_S"/>
    /// </summary>
    public FluentIlGenerator Bge_Un_S(Label label) {
        IlGenerator.Emit(OpCodes.Bge_Un_S, label);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Bgt"/>
    /// </summary>
    public FluentIlGenerator Bgt(Label label) {
        IlGenerator.Emit(OpCodes.Bgt, label);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Bgt_S"/>
    /// </summary>
    public FluentIlGenerator Bgt_S(Label label) {
        IlGenerator.Emit(OpCodes.Bgt_S, label);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Bgt_Un"/>
    /// </summary>
    public FluentIlGenerator Bgt_Un(Label label) {
        IlGenerator.Emit(OpCodes.Bgt_Un, label);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Bgt_Un_S"/>
    /// </summary>
    public FluentIlGenerator Bgt_Un_S(Label label) {
        IlGenerator.Emit(OpCodes.Bgt_Un_S, label);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Ble"/>
    /// </summary>
    public FluentIlGenerator Ble(Label label) {
        IlGenerator.Emit(OpCodes.Ble, label);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Ble_S"/>
    /// </summary>
    public FluentIlGenerator Ble_S(Label label) {
        IlGenerator.Emit(OpCodes.Ble_S, label);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Ble_Un"/>
    /// </summary>
    public FluentIlGenerator Ble_Un(Label label) {
        IlGenerator.Emit(OpCodes.Ble_Un, label);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Ble_Un_S"/>
    /// </summary>
    public FluentIlGenerator Ble_Un_S(Label label) {
        IlGenerator.Emit(OpCodes.Ble_Un_S, label);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Blt"/>
    /// </summary>
    public FluentIlGenerator Blt(Label label) {
        IlGenerator.Emit(OpCodes.Blt, label);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Blt_S"/>
    /// </summary>
    public FluentIlGenerator Blt_S(Label label) {
        IlGenerator.Emit(OpCodes.Blt_S, label);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Blt_Un"/>
    /// </summary>
    public FluentIlGenerator Blt_Un(Label label) {
        IlGenerator.Emit(OpCodes.Blt_Un, label);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Blt_Un_S"/>
    /// </summary>
    public FluentIlGenerator Blt_Un_S(Label label) {
        IlGenerator.Emit(OpCodes.Blt_Un_S, label);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Bne_Un"/>
    /// </summary>
    public FluentIlGenerator Bne_Un(Label label) {
        IlGenerator.Emit(OpCodes.Bne_Un, label);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Bne_Un_S"/>
    /// </summary>
    public FluentIlGenerator Bne_Un_S(Label label) {
        IlGenerator.Emit(OpCodes.Bne_Un_S, label);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Br"/>
    /// </summary>
    public FluentIlGenerator Br(Label label) {
        IlGenerator.Emit(OpCodes.Br, label);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Br_S"/>
    /// </summary>
    public FluentIlGenerator Br_S(Label label) {
        IlGenerator.Emit(OpCodes.Br_S, label);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Break"/>
    /// </summary>
    public FluentIlGenerator Break() {
        IlGenerator.Emit(OpCodes.Break);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Brfalse"/>
    /// </summary>
    public FluentIlGenerator Brfalse(Label label) {
        IlGenerator.Emit(OpCodes.Brfalse, label);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Brfalse_S"/>
    /// </summary>
    public FluentIlGenerator Brfalse_S(Label label) {
        IlGenerator.Emit(OpCodes.Brfalse_S, label);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Brtrue"/>
    /// </summary>
    public FluentIlGenerator Brtrue(Label label) {
        IlGenerator.Emit(OpCodes.Brtrue, label);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Brtrue_S"/>
    /// </summary>
    public FluentIlGenerator Brtrue_S(Label label) {
        IlGenerator.Emit(OpCodes.Brtrue_S, label);
        return this;
    }
    #endregion
}