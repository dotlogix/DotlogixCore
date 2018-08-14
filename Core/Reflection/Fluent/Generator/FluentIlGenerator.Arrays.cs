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
    public partial class FluentIlGenerator {
        /// <summary>
        ///     Pushes an object reference to a new zero-based, one-dimensional array whose elements are of a specific type
        ///     onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Newarr(Type elementType) {
            IlGenerator.Emit(OpCodes.Newarr);
            return this;
        }

        /// <summary>
        ///     Pushes the number of elements of a zero-based, one-dimensional array onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Ldlen() {
            IlGenerator.Emit(OpCodes.Ldlen);
            return this;
        }

        #region Load
        /// <summary>
        ///     Loads the element at a specified array index onto the top of the evaluation stack as the type specified in the
        ///     instruction.
        /// </summary>
        public FluentIlGenerator Ldelem(Type type) {
            IlGenerator.Emit(OpCodes.Ldelem, type);
            return this;
        }

        /// <summary>
        ///     Loads the element with type native int at a specified array index onto the top of the evaluation stack as
        ///     a native int.
        /// </summary>
        public FluentIlGenerator Ldelem_I() {
            IlGenerator.Emit(OpCodes.Ldelem_I);
            return this;
        }

        /// <summary>
        ///     Loads the element with type int8 at a specified array index onto the top of the evaluation stack as an int32.
        /// </summary>
        public FluentIlGenerator Ldelem_I1() {
            IlGenerator.Emit(OpCodes.Ldelem_I1);
            return this;
        }

        /// <summary>
        ///     Loads the element with type int16 at a specified array index onto the top of the evaluation stack as an int32.
        /// </summary>
        public FluentIlGenerator Ldelem_I2() {
            IlGenerator.Emit(OpCodes.Ldelem_I2);
            return this;
        }

        /// <summary>
        ///     Loads the element with type int32 at a specified array index onto the top of the evaluation stack as an int32.
        /// </summary>
        public FluentIlGenerator Ldelem_I4() {
            IlGenerator.Emit(OpCodes.Ldelem_I4);
            return this;
        }

        /// <summary>
        ///     Loads the element with type int64 at a specified array index onto the top of the evaluation stack as an int64.
        /// </summary>
        public FluentIlGenerator Ldelem_I8() {
            IlGenerator.Emit(OpCodes.Ldelem_I8);
            return this;
        }

        /// <summary>
        ///     Loads the element with type float32 at a specified array index onto the top of the evaluation stack as type
        ///     F(float).
        /// </summary>
        public FluentIlGenerator Ldelem_R4() {
            IlGenerator.Emit(OpCodes.Ldelem_R4);
            return this;
        }

        /// <summary>
        ///     Loads the element with type float64 at a specified array index onto the top of the evaluation stack as type
        ///     F(float).
        /// </summary>
        public FluentIlGenerator Ldelem_R8() {
            IlGenerator.Emit(OpCodes.Ldelem_R8);
            return this;
        }

        /// <summary>
        ///     Loads the element containing an object reference at a specified array index onto the top of the evaluation
        ///     stack as type O (object reference).
        /// </summary>
        public FluentIlGenerator Ldelem_Ref() {
            IlGenerator.Emit(OpCodes.Ldelem_Ref);
            return this;
        }

        /// <summary>
        ///     Loads the element with type unsigned int8 at a specified array index onto the top of the evaluation stack as
        ///     an int32.
        /// </summary>
        public FluentIlGenerator Ldelem_U1() {
            IlGenerator.Emit(OpCodes.Ldelem_U1);
            return this;
        }

        /// <summary>
        ///     Loads the element with type unsigned int16 at a specified array index onto the top of the evaluation stack as
        ///     an int32.
        /// </summary>
        public FluentIlGenerator Ldelem_U2() {
            IlGenerator.Emit(OpCodes.Ldelem_U2);
            return this;
        }

        /// <summary>
        ///     Loads the element with type unsigned int32 at a specified array index onto the top of the evaluation stack as
        ///     an int32.
        /// </summary>
        public FluentIlGenerator Ldelem_U4() {
            IlGenerator.Emit(OpCodes.Ldelem_U4);
            return this;
        }

        /// <summary>
        ///     Loads the address of the array element at a specified array index onto the top of the evaluation stack as type
        ///     &(managed pointer).
        /// </summary>
        public FluentIlGenerator Ldelema(Type type) {
            IlGenerator.Emit(OpCodes.Ldelema, type);
            return this;
        }
        #endregion

        #region Store
        /// <summary>
        ///     Replaces the array element at a given index with the value on the evaluation stack, whose type is specified in
        ///     the instruction.
        /// </summary>
        public FluentIlGenerator Stelem(Type type) {
            IlGenerator.Emit(OpCodes.Stelem, type);
            return this;
        }

        /// <summary>
        ///     Replaces the array element at a given index with the native int value on the evaluation stack.
        /// </summary>
        public FluentIlGenerator Stelem_I() {
            IlGenerator.Emit(OpCodes.Stelem_I);
            return this;
        }

        /// <summary>
        ///     Replaces the array element at a given index with the int8 value on the evaluation stack.
        /// </summary>
        public FluentIlGenerator Stelem_I1() {
            IlGenerator.Emit(OpCodes.Stelem_I1);
            return this;
        }

        /// <summary>
        ///     Replaces the array element at a given index with the int16 value on the evaluation stack.
        /// </summary>
        public FluentIlGenerator Stelem_I2() {
            IlGenerator.Emit(OpCodes.Stelem_I2);
            return this;
        }

        /// <summary>
        ///     Replaces the array element at a given index with the int32 value on the evaluation stack.
        /// </summary>
        public FluentIlGenerator Stelem_I4() {
            IlGenerator.Emit(OpCodes.Stelem_I4);
            return this;
        }

        /// <summary>
        ///     Replaces the array element at a given index with the int64 value on the evaluation stack.
        /// </summary>
        public FluentIlGenerator Stelem_I8() {
            IlGenerator.Emit(OpCodes.Stelem_I8);
            return this;
        }

        /// <summary>
        ///     Replaces the array element at a given index with the float32 value on the evaluation stack.
        /// </summary>
        public FluentIlGenerator Stelem_R4() {
            IlGenerator.Emit(OpCodes.Stelem_R4);
            return this;
        }

        /// <summary>
        ///     Replaces the array element at a given index with the float64 value on the evaluation stack.
        /// </summary>
        public FluentIlGenerator Stelem_R8() {
            IlGenerator.Emit(OpCodes.Stelem_R8);
            return this;
        }

        /// <summary>
        ///     Replaces the array element at a given index with the object ref value (type O) on the evaluation stack.
        /// </summary>
        public FluentIlGenerator Stelem_Ref() {
            IlGenerator.Emit(OpCodes.Stelem_Ref);
            return this;
        }
        #endregion
    }
}
