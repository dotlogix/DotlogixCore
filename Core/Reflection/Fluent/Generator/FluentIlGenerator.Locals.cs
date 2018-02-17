// ==================================================
// Copyright 2018(C) , DotLogix
// File:  FluentIlGenerator.Locals.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Reflection.Emit;
#endregion

namespace DotLogix.Core.Reflection.Fluent.Generator {
    public partial class FluentIlGenerator {
        #region Load
        /// <summary>
        ///     Loads the local variable at a specific index onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Ldloc(short index) {
            IlGenerator.Emit(OpCodes.Ldloc, index);
            return this;
        }

        /// <summary>
        ///     Loads the local variable at a specific index onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Ldloc(LocalBuilder localBuilder) {
            IlGenerator.Emit(OpCodes.Ldloc, localBuilder);
            return this;
        }

        /// <summary>
        ///     Loads the local variable at index 0 onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Ldloc_0() {
            IlGenerator.Emit(OpCodes.Ldloc_0);
            return this;
        }

        /// <summary>
        ///     Loads the local variable at index 1 onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Ldloc_1() {
            IlGenerator.Emit(OpCodes.Ldloc_1);
            return this;
        }

        /// <summary>
        ///     Loads the local variable at index 2 onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Ldloc_2() {
            IlGenerator.Emit(OpCodes.Ldloc_2);
            return this;
        }

        /// <summary>
        ///     Loads the local variable at index 3 onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Ldloc_3() {
            IlGenerator.Emit(OpCodes.Ldloc_3);
            return this;
        }

        /// <summary>
        ///     Loads the local variable at a specific index onto the evaluation stack, short form.
        /// </summary>
        public FluentIlGenerator Ldloc_S(byte index) {
            IlGenerator.Emit(OpCodes.Ldloc_S, index);
            return this;
        }

        /// <summary>
        ///     Loads the local variable at a specific index onto the evaluation stack, short form.
        /// </summary>
        public FluentIlGenerator Ldloc_S(LocalBuilder localBuilder) {
            IlGenerator.Emit(OpCodes.Ldloc_S, localBuilder);
            return this;
        }

        /// <summary>
        ///     Loads the address of the local variable at a specific index onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Ldloca(short index) {
            IlGenerator.Emit(OpCodes.Ldloca, index);
            return this;
        }

        /// <summary>
        ///     Loads the address of the local variable at a specific index onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Ldloca(LocalBuilder localBuilder) {
            IlGenerator.Emit(OpCodes.Ldloca, localBuilder);
            return this;
        }

        /// <summary>
        ///     Loads the address of the local variable at a specific index onto the evaluation stack, short form.
        /// </summary>
        public FluentIlGenerator Ldloca_S(byte index) {
            IlGenerator.Emit(OpCodes.Ldloca_S, index);
            return this;
        }

        /// <summary>
        ///     Loads the address of the local variable at a specific index onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Ldloca_S(LocalBuilder localBuilder) {
            IlGenerator.Emit(OpCodes.Ldloca_S, localBuilder);
            return this;
        }
        #endregion

        #region Store
        /// <summary>
        ///     Pops the current value from the top of the evaluation stack and stores it in a the local variable list at a
        ///     specified index.
        /// </summary>
        public FluentIlGenerator Stloc(short index) {
            IlGenerator.Emit(OpCodes.Stloc, index);
            return this;
        }

        /// <summary>
        ///     Pops the current value from the top of the evaluation stack and stores it in a the local variable list at a
        ///     specified index.
        /// </summary>
        public FluentIlGenerator Stloc(LocalBuilder localBuilder) {
            IlGenerator.Emit(OpCodes.Stloc, localBuilder);
            return this;
        }

        /// <summary>
        ///     Pops the current value from the top of the evaluation stack and stores it in a the local variable list at
        ///     index 0.
        /// </summary>
        public FluentIlGenerator Stloc_0() {
            IlGenerator.Emit(OpCodes.Stloc_0);
            return this;
        }

        /// <summary>
        ///     Pops the current value from the top of the evaluation stack and stores it in a the local variable list at
        ///     index 1.
        /// </summary>
        public FluentIlGenerator Stloc_1() {
            IlGenerator.Emit(OpCodes.Stloc_1);
            return this;
        }

        /// <summary>
        ///     Pops the current value from the top of the evaluation stack and stores it in a the local variable list at
        ///     index 2.
        /// </summary>
        public FluentIlGenerator Stloc_2() {
            IlGenerator.Emit(OpCodes.Stloc_2);
            return this;
        }

        /// <summary>
        ///     Pops the current value from the top of the evaluation stack and stores it in a the local variable list at
        ///     index 3.
        /// </summary>
        public FluentIlGenerator Stloc_3() {
            IlGenerator.Emit(OpCodes.Stloc_3);
            return this;
        }

        /// <summary>
        ///     Pops the current value from the top of the evaluation stack and stores it in a the local variable list at
        ///     index (short form).
        /// </summary>
        public FluentIlGenerator Stloc_S(byte index) {
            IlGenerator.Emit(OpCodes.Stloc_S, index);
            return this;
        }

        /// <summary>
        ///     Pops the current value from the top of the evaluation stack and stores it in a the local variable list at
        ///     index (short form).
        /// </summary>
        public FluentIlGenerator Stloc_S(LocalBuilder localBuilder) {
            IlGenerator.Emit(OpCodes.Stloc_S, localBuilder);
            return this;
        }
        #endregion
    }
}
