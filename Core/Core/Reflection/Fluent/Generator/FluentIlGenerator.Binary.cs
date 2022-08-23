// ==================================================
// Copyright 2018(C) , DotLogix
// File:  FluentIlGenerator.Binary.cs
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
    ///     <inheritdoc cref="OpCodes.And"/>
    /// </summary>
    public FluentIlGenerator And() {
        IlGenerator.Emit(OpCodes.And);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Xor"/>
    /// </summary>
    public FluentIlGenerator Xor() {
        IlGenerator.Emit(OpCodes.Xor);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Or"/>
    /// </summary>
    public FluentIlGenerator Or() {
        IlGenerator.Emit(OpCodes.Or);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Not"/>
    /// </summary>
    public FluentIlGenerator Not() {
        IlGenerator.Emit(OpCodes.Not);
        return this;
    }
}