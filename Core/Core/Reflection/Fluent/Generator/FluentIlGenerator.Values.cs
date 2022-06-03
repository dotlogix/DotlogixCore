// ==================================================
// Copyright 2018(C) , DotLogix
// File:  FluentIlGenerator.Values.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Reflection;
using System.Reflection.Emit;
#endregion

namespace DotLogix.Core.Reflection.Fluent.Generator {
    public partial class FluentIlGenerator {
        #region Objects
        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldobj"/>
        /// </summary>
        public FluentIlGenerator Ldobj(Type type) {
            IlGenerator.Emit(OpCodes.Ldobj, type);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Initobj"/>
        /// </summary>
        public FluentIlGenerator Initobj(Type type) {
            IlGenerator.Emit(OpCodes.Initobj, type);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Newobj"/>
        /// </summary>
        public FluentIlGenerator Newobj(ConstructorInfo ctorInfo) {
            IlGenerator.Emit(OpCodes.Newobj, ctorInfo);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Stobj"/>
        /// </summary>
        public FluentIlGenerator Stobj(Type type) {
            IlGenerator.Emit(OpCodes.Stobj, type);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Isinst"/>
        /// </summary>
        public FluentIlGenerator Isinst(Type type) {
            IlGenerator.Emit(OpCodes.Isinst, type);
            return this;
        }
        #endregion

        #region Push
        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldnull"/>
        /// </summary>
        public FluentIlGenerator Ldnull() {
            IlGenerator.Emit(OpCodes.Ldnull);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldstr"/>
        /// </summary>
        public FluentIlGenerator Ldstr(string value) {
            IlGenerator.Emit(OpCodes.Ldstr, value);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldc_I4"/>
        /// </summary>
        public FluentIlGenerator Ldc_I4(int value) {
            IlGenerator.Emit(OpCodes.Ldc_I4, value);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldc_I4_0"/>
        /// </summary>
        public FluentIlGenerator Ldc_I4_0() {
            IlGenerator.Emit(OpCodes.Ldc_I4_0);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldc_I4_1"/>
        /// </summary>
        public FluentIlGenerator Ldc_I4_1() {
            IlGenerator.Emit(OpCodes.Ldc_I4_1);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldc_I4_2"/>
        /// </summary>
        public FluentIlGenerator Ldc_I4_2() {
            IlGenerator.Emit(OpCodes.Ldc_I4_2);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldc_I4_3"/>
        /// </summary>
        public FluentIlGenerator Ldc_I4_3() {
            IlGenerator.Emit(OpCodes.Ldc_I4_3);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldc_I4_4"/>
        /// </summary>
        public FluentIlGenerator Ldc_I4_4() {
            IlGenerator.Emit(OpCodes.Ldc_I4_4);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldc_I4_5"/>
        /// </summary>
        public FluentIlGenerator Ldc_I4_5() {
            IlGenerator.Emit(OpCodes.Ldc_I4_5);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldc_I4_6"/>
        /// </summary>
        public FluentIlGenerator Ldc_I4_6() {
            IlGenerator.Emit(OpCodes.Ldc_I4_6);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldc_I4_7"/>
        /// </summary>
        public FluentIlGenerator Ldc_I4_7() {
            IlGenerator.Emit(OpCodes.Ldc_I4_7);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldc_I4_8"/>
        /// </summary>
        public FluentIlGenerator Ldc_I4_8() {
            IlGenerator.Emit(OpCodes.Ldc_I4_8);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldc_I4_M1"/>
        /// </summary>
        public FluentIlGenerator Ldc_I4_M1() {
            IlGenerator.Emit(OpCodes.Ldc_I4_M1);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldc_I4_S"/>
        /// </summary>
        public FluentIlGenerator Ldc_I4_S(sbyte value) {
            IlGenerator.Emit(OpCodes.Ldc_I4_S, value);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldc_I8"/>
        /// </summary>
        public FluentIlGenerator Ldc_I8(long value) {
            IlGenerator.Emit(OpCodes.Ldc_I8, value);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldc_R4"/>
        /// </summary>
        public FluentIlGenerator Ldc_R4(float value) {
            IlGenerator.Emit(OpCodes.Ldc_R4, value);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldc_R8"/>
        /// </summary>
        public FluentIlGenerator Ldc_R8(double value) {
            IlGenerator.Emit(OpCodes.Ldc_R8, value);
            return this;
        }
        #endregion
    }
}
