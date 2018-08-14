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

namespace DotLogix.Core.Reflection.Fluent.Generator {
    public partial class FluentIlGenerator {
        /// <summary>
        ///     Computes the bitwise AND of two values and pushes the result onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator And() {
            IlGenerator.Emit(OpCodes.And);
            return this;
        }

        /// <summary>
        ///     Computes the bitwise NAND of two values and pushes the result onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Nand() {
            IlGenerator.Emit(OpCodes.And);
            IlGenerator.Emit(OpCodes.Not);
            return this;
        }

        /// <summary>
        ///     Computes the bitwise XOR of the top two values on the evaluation stack, pushing the result onto the evaluation
        ///     stack.
        /// </summary>
        public FluentIlGenerator Xor() {
            IlGenerator.Emit(OpCodes.Xor);
            return this;
        }

        /// <summary>
        ///     Computes the bitwise XNOR of the top two values on the evaluation stack, pushing the result onto the evaluation
        ///     stack.
        /// </summary>
        public FluentIlGenerator Xnor() {
            IlGenerator.Emit(OpCodes.Xor);
            IlGenerator.Emit(OpCodes.Not);
            return this;
        }

        /// <summary>
        ///     Computes the bitwise OR of two values and pushes the result onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Or() {
            IlGenerator.Emit(OpCodes.Or);
            return this;
        }

        /// <summary>
        ///     Computes the bitwise NOR of two values and pushes the result onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Nor() {
            IlGenerator.Emit(OpCodes.Or);
            IlGenerator.Emit(OpCodes.Not);
            return this;
        }

        /// <summary>
        ///     Inverts the top value on the evaluation stack, pushing the result onto the evaluation
        ///     stack.
        /// </summary>
        public FluentIlGenerator Not() {
            IlGenerator.Emit(OpCodes.Not);
            return this;
        }
    }
}
