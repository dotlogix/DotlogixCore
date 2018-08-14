// ==================================================
// Copyright 2018(C) , DotLogix
// File:  FluentIlGenerator.Comparison.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Reflection.Emit;
#endregion

namespace DotLogix.Core.Reflection.Fluent.Generator {
    public partial class FluentIlGenerator {
        /// <summary>
        ///     Compares two values. If they are equal, the integer value 1 (int32) is pushed onto the evaluation stack;
        ///     otherwise 0 (int32) is pushed onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Ceq() {
            IlGenerator.Emit(OpCodes.Ceq);
            return this;
        }

        /// <summary>
        ///     Compares two values. If the first value is greater than the second, the integer value 1 (int32) is pushed onto
        ///     the evaluation stack; otherwise 0 (int32) is pushed onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Cgt() {
            IlGenerator.Emit(OpCodes.Cgt);
            return this;
        }

        /// <summary>
        ///     Compares two unsigned or unordered values. If the first value is greater than the second, the integer value 1
        ///     (int32) is pushed onto the evaluation stack; otherwise 0 (int32) is pushed onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Cgt_Un() {
            IlGenerator.Emit(OpCodes.Cgt_Un);
            return this;
        }

        /// <summary>
        ///     Compares two values. If the first value is less than the second, the integer value 1 (int32) is pushed onto
        ///     the evaluation stack; otherwise 0 (int32) is pushed onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Clt() {
            IlGenerator.Emit(OpCodes.Clt);
            return this;
        }

        /// <summary>
        ///     Compares the unsigned or unordered values value1 and value2. If value1 is less than value2, then the integer
        ///     value 1 (int32) is pushed onto the evaluation stack; otherwise 0 (int32) is pushed onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Clt_Un() {
            IlGenerator.Emit(OpCodes.Clt_Un);
            return this;
        }

        /// <summary>
        ///     Throws ArithmeticException if value is not a finite number.
        /// </summary>
        public FluentIlGenerator Ckfinite() {
            IlGenerator.Emit(OpCodes.Ckfinite);
            return this;
        }
    }
}
