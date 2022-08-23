// ==================================================
// Copyright 2018(C) , DotLogix
// File:  FluentIlGenerator.Fields.cs
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
    #region Load
    /// <summary>
    ///     <inheritdoc cref="OpCodes.Ldfld"/>
    /// </summary>
    public FluentIlGenerator Ldfld(FieldInfo fieldInfo) {
        IlGenerator.Emit(OpCodes.Ldfld, fieldInfo);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Ldflda"/>
    /// </summary>
    public FluentIlGenerator Ldflda(FieldInfo fieldInfo) {
        IlGenerator.Emit(OpCodes.Ldflda, fieldInfo);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Ldsfld"/>
    /// </summary>
    public FluentIlGenerator Ldsfld(FieldInfo fieldInfo) {
        IlGenerator.Emit(OpCodes.Ldsfld, fieldInfo);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Ldsflda"/>
    /// </summary>
    public FluentIlGenerator Ldsflda(FieldInfo fieldInfo) {
        IlGenerator.Emit(OpCodes.Ldsflda, fieldInfo);
        return this;
    }
    #endregion

    #region Store
    /// <summary>
    ///     <inheritdoc cref="OpCodes.Stfld"/>
    /// </summary>
    public FluentIlGenerator Stfld(FieldInfo fieldInfo) {
        IlGenerator.Emit(OpCodes.Stfld, fieldInfo);
        return this;
    }

    /// <summary>
    ///     <inheritdoc cref="OpCodes.Stsfld"/>
    /// </summary>
    public FluentIlGenerator Stsfld(FieldInfo fieldInfo) {
        IlGenerator.Emit(OpCodes.Stsfld, fieldInfo);
        return this;
    }
    #endregion
}