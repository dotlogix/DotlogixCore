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
        ///     Copies the value type object pointed to by an address to the top of the evaluation stack.
        /// </summary>
        public FluentIlGenerator Ldobj(Type type) {
            IlGenerator.Emit(OpCodes.Ldobj, type);
            return this;
        }

        /// <summary>
        ///     Initializes each field of the value type at a specified address to a null reference or a 0 of the appropriate
        ///     primitive type.
        /// </summary>
        public FluentIlGenerator Initobj(Type type) {
            IlGenerator.Emit(OpCodes.Initobj, type);
            return this;
        }

        /// <summary>
        ///     Creates a new object or a new instance of a value type, pushing an object reference (type O) onto the
        ///     evaluation stack.
        /// </summary>
        public FluentIlGenerator Newobj(ConstructorInfo ctorInfo) {
            IlGenerator.Emit(OpCodes.Newobj, ctorInfo);
            return this;
        }

        /// <summary>
        ///     Copies a value of a specified type from the evaluation stack into a supplied memory address.
        /// </summary>
        public FluentIlGenerator Stobj(Type type) {
            IlGenerator.Emit(OpCodes.Stobj, type);
            return this;
        }

        /// <summary>
        ///     Tests whether an object reference (type O) is an instance of a particular class.
        /// </summary>
        public FluentIlGenerator Isinst(Type type) {
            IlGenerator.Emit(OpCodes.Isinst, type);
            return this;
        }
        #endregion

        #region Push
        /// <summary>
        ///     Pushes a null reference (type O) onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Ldnull() {
            IlGenerator.Emit(OpCodes.Ldnull);
            return this;
        }

        /// <summary>
        ///     Pushes a new object reference to a string literal stored in the metadata.
        /// </summary>
        public FluentIlGenerator Ldstr(string value) {
            IlGenerator.Emit(OpCodes.Ldstr, value);
            return this;
        }

        /// <summary>
        ///     Pushes a supplied value of type int32 onto the evaluation stack as an int32.
        /// </summary>
        public FluentIlGenerator Ldc_I4(int value) {
            IlGenerator.Emit(OpCodes.Ldc_I4, value);
            return this;
        }

        /// <summary>
        ///     Pushes the integer value of 0 onto the evaluation stack as an int32.
        /// </summary>
        public FluentIlGenerator Ldc_I4_0() {
            IlGenerator.Emit(OpCodes.Ldc_I4_0);
            return this;
        }

        /// <summary>
        ///     Pushes the integer value of 1 onto the evaluation stack as an int32.
        /// </summary>
        public FluentIlGenerator Ldc_I4_1() {
            IlGenerator.Emit(OpCodes.Ldc_I4_1);
            return this;
        }

        /// <summary>
        ///     Pushes the integer value of 2 onto the evaluation stack as an int32.
        /// </summary>
        public FluentIlGenerator Ldc_I4_2() {
            IlGenerator.Emit(OpCodes.Ldc_I4_2);
            return this;
        }

        /// <summary>
        ///     Pushes the integer value of 3 onto the evaluation stack as an int32.
        /// </summary>
        public FluentIlGenerator Ldc_I4_3() {
            IlGenerator.Emit(OpCodes.Ldc_I4_3);
            return this;
        }

        /// <summary>
        ///     Pushes the integer value of 4 onto the evaluation stack as an int32.
        /// </summary>
        public FluentIlGenerator Ldc_I4_4() {
            IlGenerator.Emit(OpCodes.Ldc_I4_4);
            return this;
        }

        /// <summary>
        ///     Pushes the integer value of 5 onto the evaluation stack as an int32.
        /// </summary>
        public FluentIlGenerator Ldc_I4_5() {
            IlGenerator.Emit(OpCodes.Ldc_I4_5);
            return this;
        }

        /// <summary>
        ///     Pushes the integer value of 6 onto the evaluation stack as an int32.
        /// </summary>
        public FluentIlGenerator Ldc_I4_6() {
            IlGenerator.Emit(OpCodes.Ldc_I4_6);
            return this;
        }

        /// <summary>
        ///     Pushes the integer value of 7 onto the evaluation stack as an int32.
        /// </summary>
        public FluentIlGenerator Ldc_I4_7() {
            IlGenerator.Emit(OpCodes.Ldc_I4_7);
            return this;
        }

        /// <summary>
        ///     Pushes the integer value of 8 onto the evaluation stack as an int32.
        /// </summary>
        public FluentIlGenerator Ldc_I4_8() {
            IlGenerator.Emit(OpCodes.Ldc_I4_8);
            return this;
        }

        /// <summary>
        ///     Pushes the integer value of -1 onto the evaluation stack as an int32.
        /// </summary>
        public FluentIlGenerator Ldc_I4_M1() {
            IlGenerator.Emit(OpCodes.Ldc_I4_M1);
            return this;
        }

        /// <summary>
        ///     Pushes the supplied int8 value onto the evaluation stack as an int32, short form.
        /// </summary>
        public FluentIlGenerator Ldc_I4_S(sbyte value) {
            IlGenerator.Emit(OpCodes.Ldc_I4_S, value);
            return this;
        }

        /// <summary>
        ///     Pushes a supplied value of type int64 onto the evaluation stack as an int64.
        /// </summary>
        public FluentIlGenerator Ldc_I8(long value) {
            IlGenerator.Emit(OpCodes.Ldc_I8, value);
            return this;
        }

        /// <summary>
        ///     Pushes a supplied value of type float32 onto the evaluation stack as type F (float).
        /// </summary>
        public FluentIlGenerator Ldc_R4(float value) {
            IlGenerator.Emit(OpCodes.Ldc_R4, value);
            return this;
        }

        /// <summary>
        ///     Pushes a supplied value of type float64 onto the evaluation stack as type F (float).
        /// </summary>
        public FluentIlGenerator Ldc_R8(double value) {
            IlGenerator.Emit(OpCodes.Ldc_R8, value);
            return this;
        }
        #endregion
    }
}
