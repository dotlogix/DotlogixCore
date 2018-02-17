// ==================================================
// Copyright 2018(C) , DotLogix
// File:  FluentIlGenerator.Convert.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Reflection.Emit;
#endregion

namespace DotLogix.Core.Reflection.Fluent.Generator {
    public partial class FluentIlGenerator {
        #region Convert
        /// <summary>
        ///     Converts the value on top of the evaluation stack to native int.
        /// </summary>
        public FluentIlGenerator Conv_I() {
            IlGenerator.Emit(OpCodes.Conv_I);
            return this;
        }

        /// <summary>
        ///     Converts the value on top of the evaluation stack to int8, then extends (pads) it to int32.
        /// </summary>
        public FluentIlGenerator Conv_I1() {
            IlGenerator.Emit(OpCodes.Conv_I1);
            return this;
        }

        /// <summary>
        ///     Converts the value on top of the evaluation stack to int16, then extends (pads) it to int32.
        /// </summary>
        public FluentIlGenerator Conv_I2() {
            IlGenerator.Emit(OpCodes.Conv_I2);
            return this;
        }

        /// <summary>
        ///     Converts the value on top of the evaluation stack to int32.
        /// </summary>
        public FluentIlGenerator Conv_I4() {
            IlGenerator.Emit(OpCodes.Conv_I4);
            return this;
        }

        /// <summary>
        ///     Converts the value on top of the evaluation stack to int64.
        /// </summary>
        public FluentIlGenerator Conv_I8() {
            IlGenerator.Emit(OpCodes.Conv_I8);
            return this;
        }

        /// <summary>
        ///     Converts the signed value on top of the evaluation stack to signed native int, throwing OverflowException on
        ///     overflow.
        /// </summary>
        public FluentIlGenerator Conv_Ovf_I() {
            IlGenerator.Emit(OpCodes.Conv_Ovf_I);
            return this;
        }

        /// <summary>
        ///     Converts the unsigned value on top of the evaluation stack to signed native int, throwing OverflowException on
        ///     overflow.
        /// </summary>
        public FluentIlGenerator Conv_Ovf_I_Un() {
            IlGenerator.Emit(OpCodes.Conv_Ovf_I_Un);
            return this;
        }

        /// <summary>
        ///     Converts the signed value on top of the evaluation stack to signed int8 and extends it to int32, throwing
        ///     OverflowException on overflow.
        /// </summary>
        public FluentIlGenerator Conv_Ovf_I1() {
            IlGenerator.Emit(OpCodes.Conv_Ovf_I1);
            return this;
        }

        /// <summary>
        ///     Converts the unsigned value on top of the evaluation stack to signed int8 and extends it to int32, throwing
        ///     OverflowException on overflow.
        /// </summary>
        public FluentIlGenerator Conv_Ovf_I1_Un() {
            IlGenerator.Emit(OpCodes.Conv_Ovf_I1_Un);
            return this;
        }

        /// <summary>
        ///     Converts the signed value on top of the evaluation stack to signed int16 and extending it to int32, throwing
        ///     OverflowException on overflow.
        /// </summary>
        public FluentIlGenerator Conv_Ovf_I2() {
            IlGenerator.Emit(OpCodes.Conv_Ovf_I2);
            return this;
        }

        /// <summary>
        ///     Converts the unsigned value on top of the evaluation stack to signed int16 and extends it to int32, throwing
        ///     OverflowException on overflow.
        /// </summary>
        public FluentIlGenerator Conv_Ovf_I2_Un() {
            IlGenerator.Emit(OpCodes.Conv_Ovf_I2_Un);
            return this;
        }

        /// <summary>
        ///     Converts the signed value on top of the evaluation stack to signed int32, throwing OverflowException on
        ///     overflow.
        /// </summary>
        public FluentIlGenerator Conv_Ovf_I4() {
            IlGenerator.Emit(OpCodes.Conv_Ovf_I4);
            return this;
        }

        /// <summary>
        ///     Converts the unsigned value on top of the evaluation stack to signed int32, throwing OverflowException on
        ///     overflow.
        /// </summary>
        public FluentIlGenerator Conv_Ovf_I4_Un() {
            IlGenerator.Emit(OpCodes.Conv_Ovf_I4_Un);
            return this;
        }

        /// <summary>
        ///     Converts the signed value on top of the evaluation stack to signed int64, throwing OverflowException on
        ///     overflow.
        /// </summary>
        public FluentIlGenerator Conv_Ovf_I8() {
            IlGenerator.Emit(OpCodes.Conv_Ovf_I8);
            return this;
        }

        /// <summary>
        ///     Converts the unsigned value on top of the evaluation stack to signed int64, throwing OverflowException on
        ///     overflow.
        /// </summary>
        public FluentIlGenerator Conv_Ovf_I8_Un() {
            IlGenerator.Emit(OpCodes.Conv_Ovf_I8_Un);
            return this;
        }

        /// <summary>
        ///     Converts the signed value on top of the evaluation stack to unsigned native int, throwing OverflowExceptionon
        ///     overflow.
        /// </summary>
        public FluentIlGenerator Conv_Ovf_U() {
            IlGenerator.Emit(OpCodes.Conv_Ovf_U);
            return this;
        }

        /// <summary>
        ///     Converts the unsigned value on top of the evaluation stack to unsigned native int, throwing
        ///     OverflowExceptionon overflow.
        /// </summary>
        public FluentIlGenerator Conv_Ovf_U_Un() {
            IlGenerator.Emit(OpCodes.Conv_Ovf_U_Un);
            return this;
        }

        /// <summary>
        ///     Converts the signed value on top of the evaluation stack to unsigned int8 and extends it to int32, throwing
        ///     OverflowException on overflow.
        /// </summary>
        public FluentIlGenerator Conv_Ovf_U1() {
            IlGenerator.Emit(OpCodes.Conv_Ovf_U1);
            return this;
        }

        /// <summary>
        ///     Converts the unsigned value on top of the evaluation stack to unsigned int8 and extends it to int32, throwing
        ///     OverflowException on overflow.
        /// </summary>
        public FluentIlGenerator Conv_Ovf_U1_Un() {
            IlGenerator.Emit(OpCodes.Conv_Ovf_U1_Un);
            return this;
        }

        /// <summary>
        ///     Converts the signed value on top of the evaluation stack to unsigned int16 and extends it to int32, throwing
        ///     OverflowException on overflow.
        /// </summary>
        public FluentIlGenerator Conv_Ovf_U2() {
            IlGenerator.Emit(OpCodes.Conv_Ovf_U2);
            return this;
        }

        /// <summary>
        ///     Converts the unsigned value on top of the evaluation stack to unsigned int16 and extends it to int32, throwing
        ///     OverflowException on overflow.
        /// </summary>
        public FluentIlGenerator Conv_Ovf_U2_Un() {
            IlGenerator.Emit(OpCodes.Conv_Ovf_U2_Un);
            return this;
        }

        /// <summary>
        ///     Converts the signed value on top of the evaluation stack to unsigned int32, throwing OverflowException on
        ///     overflow.
        /// </summary>
        public FluentIlGenerator Conv_Ovf_U4() {
            IlGenerator.Emit(OpCodes.Conv_Ovf_U4);
            return this;
        }

        /// <summary>
        ///     Converts the unsigned value on top of the evaluation stack to unsigned int32, throwing OverflowException on
        ///     overflow.
        /// </summary>
        public FluentIlGenerator Conv_Ovf_U4_Un() {
            IlGenerator.Emit(OpCodes.Conv_Ovf_U4_Un);
            return this;
        }

        /// <summary>
        ///     Converts the signed value on top of the evaluation stack to unsigned int64, throwing OverflowException on
        ///     overflow.
        /// </summary>
        public FluentIlGenerator Conv_Ovf_U8() {
            IlGenerator.Emit(OpCodes.Conv_Ovf_U8);
            return this;
        }

        /// <summary>
        ///     Converts the unsigned value on top of the evaluation stack to unsigned int64, throwing OverflowException on
        ///     overflow.
        /// </summary>
        public FluentIlGenerator Conv_Ovf_U8_Un() {
            IlGenerator.Emit(OpCodes.Conv_Ovf_U8_Un);
            return this;
        }

        /// <summary>
        ///     Converts the unsigned integer value on top of the evaluation stack to float32.
        /// </summary>
        public FluentIlGenerator Conv_R_Un() {
            IlGenerator.Emit(OpCodes.Conv_R_Un);
            return this;
        }

        /// <summary>
        ///     Converts the value on top of the evaluation stack to float32.
        /// </summary>
        public FluentIlGenerator Conv_R4() {
            IlGenerator.Emit(OpCodes.Conv_R4);
            return this;
        }

        /// <summary>
        ///     Converts the value on top of the evaluation stack to float64.
        /// </summary>
        public FluentIlGenerator Conv_R8() {
            IlGenerator.Emit(OpCodes.Conv_R8);
            return this;
        }

        /// <summary>
        ///     Converts the value on top of the evaluation stack to unsigned native int, and extends it to native int.
        /// </summary>
        public FluentIlGenerator Conv_U() {
            IlGenerator.Emit(OpCodes.Conv_U);
            return this;
        }

        /// <summary>
        ///     Converts the value on top of the evaluation stack to unsigned int8, and extends it to int32.
        /// </summary>
        public FluentIlGenerator Conv_U1() {
            IlGenerator.Emit(OpCodes.Conv_U1);
            return this;
        }

        /// <summary>
        ///     Converts the value on top of the evaluation stack to unsigned int16, and extends it to int32.
        /// </summary>
        public FluentIlGenerator Conv_U2() {
            IlGenerator.Emit(OpCodes.Conv_U2);
            return this;
        }

        /// <summary>
        ///     Converts the value on top of the evaluation stack to unsigned int32, and extends it to int32.
        /// </summary>
        public FluentIlGenerator Conv_U4() {
            IlGenerator.Emit(OpCodes.Conv_U4);
            return this;
        }

        /// <summary>
        ///     Converts the value on top of the evaluation stack to unsigned int64, and extends it to int64.
        /// </summary>
        public FluentIlGenerator Conv_U8() {
            IlGenerator.Emit(OpCodes.Conv_U8);
            return this;
        }
        #endregion

        #region Boxing
        /// <summary>
        ///     Converts a value type to an object reference (type O).
        /// </summary>
        public FluentIlGenerator Box(Type type) {
            IlGenerator.Emit(OpCodes.Box, type);
            return this;
        }

        /// <summary>
        ///     Converts the boxed representation of a value type to its unboxed form.
        /// </summary>
        public FluentIlGenerator Unbox(Type type) {
            IlGenerator.Emit(OpCodes.Unbox, type);
            return this;
        }

        /// <summary>
        ///     Converts the boxed representation of a type specified in the instruction to its unboxed form.
        /// </summary>
        public FluentIlGenerator Unbox_Any(Type type) {
            IlGenerator.Emit(OpCodes.Unbox_Any, type);
            return this;
        }

        /// <summary>
        ///     Attempts to cast an object passed by reference to the specified class.
        /// </summary>
        public FluentIlGenerator Castclass(Type type) {
            IlGenerator.Emit(OpCodes.Castclass, type);
            return this;
        }
        #endregion
    }
}
