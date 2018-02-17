// ==================================================
// Copyright 2018(C) , DotLogix
// File:  FluentIlGenerator.Math.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Reflection.Emit;
#endregion

namespace DotLogix.Core.Reflection.Fluent.Generator {
    public partial class FluentIlGenerator {
        /// <summary>
        ///     Shifts an integer value to the left (in zeroes) by a specified number of bits, pushing the result onto the
        ///     evaluation stack.
        /// </summary>
        public FluentIlGenerator Shl() {
            IlGenerator.Emit(OpCodes.Shl);
            return this;
        }

        /// <summary>
        ///     Shifts an integer value (in sign) to the right by a specified number of bits, pushing the result onto the
        ///     evaluation stack.
        /// </summary>
        public FluentIlGenerator Shr() {
            IlGenerator.Emit(OpCodes.Shr);
            return this;
        }

        /// <summary>
        ///     Shifts an unsigned integer value (in zeroes) to the right by a specified number of bits, pushing the result
        ///     onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Shr_Un() {
            IlGenerator.Emit(OpCodes.Shr_Un);
            return this;
        }

        /// <summary>
        ///     Divides two values and pushes the remainder onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Rem() {
            IlGenerator.Emit(OpCodes.Rem);
            return this;
        }

        /// <summary>
        ///     Divides two unsigned values and pushes the remainder onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Rem_Un() {
            IlGenerator.Emit(OpCodes.Rem_Un);
            return this;
        }

        /// <summary>
        ///     Subtracts one value from another and pushes the result onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Sub() {
            IlGenerator.Emit(OpCodes.Sub);
            return this;
        }

        /// <summary>
        ///     Subtracts one integer value from another, performs an overflow check, and pushes the result onto the
        ///     evaluation stack.
        /// </summary>
        public FluentIlGenerator Sub_Ovf() {
            IlGenerator.Emit(OpCodes.Sub_Ovf);
            return this;
        }

        /// <summary>
        ///     Subtracts one unsigned integer value from another, performs an overflow check, and pushes the result onto the
        ///     evaluation stack.
        /// </summary>
        public FluentIlGenerator Sub_Ovf_Un() {
            IlGenerator.Emit(OpCodes.Sub_Ovf_Un);
            return this;
        }

        /// <summary>
        ///     Adds two values and pushes the result onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Add() {
            IlGenerator.Emit(OpCodes.Add);
            return this;
        }

        /// <summary>
        ///     Adds two integers, performs an overflow check, and pushes the result onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Add_Ovf() {
            IlGenerator.Emit(OpCodes.Add_Ovf);
            return this;
        }

        /// <summary>
        ///     Adds two unsigned integer values, performs an overflow check, and pushes the result onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Add_Ovf_Un() {
            IlGenerator.Emit(OpCodes.Add_Ovf_Un);
            return this;
        }

        /// <summary>
        ///     Divides two values and pushes the result as a floating-point (type F) or quotient (type int32) onto the
        ///     evaluation stack.
        /// </summary>
        public FluentIlGenerator Div() {
            IlGenerator.Emit(OpCodes.Div);
            return this;
        }

        /// <summary>
        ///     Divides two unsigned integer values and pushes the result (int32) onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Div_Un() {
            IlGenerator.Emit(OpCodes.Div_Un);
            return this;
        }

        /// <summary>
        ///     Multiplies two values and pushes the result on the evaluation stack.
        /// </summary>
        public FluentIlGenerator Mul() {
            IlGenerator.Emit(OpCodes.Mul);
            return this;
        }

        /// <summary>
        ///     Multiplies two integer values, performs an overflow check, and pushes the result onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Mul_Ovf() {
            IlGenerator.Emit(OpCodes.Mul_Ovf);
            return this;
        }

        /// <summary>
        ///     Multiplies two unsigned integer values, performs an overflow check, and pushes the result onto the evaluation
        ///     stack.
        /// </summary>
        public FluentIlGenerator Mul_Ovf_Un() {
            IlGenerator.Emit(OpCodes.Mul_Ovf_Un);
            return this;
        }

        /// <summary>
        ///     Negates a value and pushes the result onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Neg() {
            IlGenerator.Emit(OpCodes.Neg);
            return this;
        }
    }
}
