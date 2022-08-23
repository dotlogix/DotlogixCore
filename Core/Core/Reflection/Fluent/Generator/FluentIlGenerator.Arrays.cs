// ==================================================
// Copyright 2018(C) , DotLogix
// File:  FluentIlGenerator.Arrays.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Reflection.Emit;
#endregion

namespace DotLogix.Core.Reflection.Fluent.Generator {
    /// <summary>
    /// A fluent il generator
    /// </summary>
    public partial class FluentIlGenerator {
        /// <summary>
        ///     <inheritdoc cref="OpCodes.Newarr"/>
        /// </summary>
        public FluentIlGenerator Newarr(Type elementType) {
            IlGenerator.Emit(OpCodes.Newarr, elementType);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldlen"/>
        /// </summary>
        public FluentIlGenerator Ldlen() {
            IlGenerator.Emit(OpCodes.Ldlen);
            return this;
        }

        #region Load
        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldelem"/>
        /// </summary>
        public FluentIlGenerator Ldelem(Type type) {
            IlGenerator.Emit(OpCodes.Ldelem, type);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldelem_I"/>
        /// </summary>
        public FluentIlGenerator Ldelem_I() {
            IlGenerator.Emit(OpCodes.Ldelem_I);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldelem_I1"/>
        /// </summary>
        public FluentIlGenerator Ldelem_I1() {
            IlGenerator.Emit(OpCodes.Ldelem_I1);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldelem_I2"/>
        /// </summary>
        public FluentIlGenerator Ldelem_I2() {
            IlGenerator.Emit(OpCodes.Ldelem_I2);
            return this;
        }
        
        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldelem_I4"/>
        /// </summary>
        public FluentIlGenerator Ldelem_I4() {
            IlGenerator.Emit(OpCodes.Ldelem_I4);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldelem_I8"/>
        /// </summary>
        public FluentIlGenerator Ldelem_I8() {
            IlGenerator.Emit(OpCodes.Ldelem_I8);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldelem_R4"/>
        /// </summary>
        public FluentIlGenerator Ldelem_R4() {
            IlGenerator.Emit(OpCodes.Ldelem_R4);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldelem_R8"/>
        /// </summary>
        public FluentIlGenerator Ldelem_R8() {
            IlGenerator.Emit(OpCodes.Ldelem_R8);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldelem_Ref"/>
        /// </summary>
        public FluentIlGenerator Ldelem_Ref() {
            IlGenerator.Emit(OpCodes.Ldelem_Ref);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldelem_U1"/>
        /// </summary>
        public FluentIlGenerator Ldelem_U1() {
            IlGenerator.Emit(OpCodes.Ldelem_U1);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldelem_U2"/>
        /// </summary>
        public FluentIlGenerator Ldelem_U2() {
            IlGenerator.Emit(OpCodes.Ldelem_U2);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldelem_U4"/>
        /// </summary>
        public FluentIlGenerator Ldelem_U4() {
            IlGenerator.Emit(OpCodes.Ldelem_U4);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldelema"/>
        /// </summary>
        public FluentIlGenerator Ldelema(Type type) {
            IlGenerator.Emit(OpCodes.Ldelema, type);
            return this;
        }
        #endregion

        #region Store
        /// <summary>
        ///     <inheritdoc cref="OpCodes.Stelem"/>
        /// </summary>
        public FluentIlGenerator Stelem(Type type) {
            IlGenerator.Emit(OpCodes.Stelem, type);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Stelem_I"/>
        /// </summary>
        public FluentIlGenerator Stelem_I() {
            IlGenerator.Emit(OpCodes.Stelem_I);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Stelem_I1"/>
        /// </summary>
        public FluentIlGenerator Stelem_I1() {
            IlGenerator.Emit(OpCodes.Stelem_I1);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Stelem_I2"/>
        /// </summary>
        public FluentIlGenerator Stelem_I2() {
            IlGenerator.Emit(OpCodes.Stelem_I2);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Stelem_I4"/>
        /// </summary>
        public FluentIlGenerator Stelem_I4() {
            IlGenerator.Emit(OpCodes.Stelem_I4);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Stelem_I8"/>
        /// </summary>
        public FluentIlGenerator Stelem_I8() {
            IlGenerator.Emit(OpCodes.Stelem_I8);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Stelem_R4"/>
        /// </summary>
        public FluentIlGenerator Stelem_R4() {
            IlGenerator.Emit(OpCodes.Stelem_R4);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Stelem_R8"/>
        /// </summary>
        public FluentIlGenerator Stelem_R8() {
            IlGenerator.Emit(OpCodes.Stelem_R8);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Stelem_Ref"/>
        /// </summary>
        public FluentIlGenerator Stelem_Ref() {
            IlGenerator.Emit(OpCodes.Stelem_Ref);
            return this;
        }
        #endregion
    }
}