// ==================================================
// Copyright 2018(C) , DotLogix
// File:  FluentIlGenerator.Indirect.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Reflection.Emit;
#endregion

namespace DotLogix.Core.Reflection.Fluent.Generator {
    public partial class FluentIlGenerator {
        #region Store
        /// <summary>
        ///     <inheritdoc cref="OpCodes.Stind_I"/>
        /// </summary>
        public FluentIlGenerator Stind_I() {
            IlGenerator.Emit(OpCodes.Stind_I);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Stind_I1"/>
        /// </summary>
        public FluentIlGenerator Stind_I1() {
            IlGenerator.Emit(OpCodes.Stind_I1);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Stind_I2"/>
        /// </summary>
        public FluentIlGenerator Stind_I2() {
            IlGenerator.Emit(OpCodes.Stind_I2);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Stind_I4"/>
        /// </summary>
        public FluentIlGenerator Stind_I4() {
            IlGenerator.Emit(OpCodes.Stind_I4);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Stind_I8"/>
        /// </summary>
        public FluentIlGenerator Stind_I8() {
            IlGenerator.Emit(OpCodes.Stind_I8);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Stind_R4"/>
        /// </summary>
        public FluentIlGenerator Stind_R4() {
            IlGenerator.Emit(OpCodes.Stind_R4);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Stind_R8"/>
        /// </summary>
        public FluentIlGenerator Stind_R8() {
            IlGenerator.Emit(OpCodes.Stind_R8);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Stind_Ref"/>
        /// </summary>
        public FluentIlGenerator Stind_Ref() {
            IlGenerator.Emit(OpCodes.Stind_Ref);
            return this;
        }
        #endregion

        #region Load
        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldind_I"/>
        /// </summary>
        public FluentIlGenerator Ldind_I() {
            IlGenerator.Emit(OpCodes.Ldind_I);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldind_I1"/>
        /// </summary>
        public FluentIlGenerator Ldind_I1() {
            IlGenerator.Emit(OpCodes.Ldind_I1);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldind_I2"/>
        /// </summary>
        public FluentIlGenerator Ldind_I2() {
            IlGenerator.Emit(OpCodes.Ldind_I2);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldind_I4"/>
        /// </summary>
        public FluentIlGenerator Ldind_I4() {
            IlGenerator.Emit(OpCodes.Ldind_I4);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldind_I8"/>
        /// </summary>
        public FluentIlGenerator Ldind_I8() {
            IlGenerator.Emit(OpCodes.Ldind_I8);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldind_R4"/>
        /// </summary>
        public FluentIlGenerator Ldind_R4() {
            IlGenerator.Emit(OpCodes.Ldind_R4);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldind_R8"/>
        /// </summary>
        public FluentIlGenerator Ldind_R8() {
            IlGenerator.Emit(OpCodes.Ldind_R8);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldind_Ref"/>
        /// </summary>
        public FluentIlGenerator Ldind_Ref() {
            IlGenerator.Emit(OpCodes.Ldind_Ref);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldind_U1"/>
        /// </summary>
        public FluentIlGenerator Ldind_U1() {
            IlGenerator.Emit(OpCodes.Ldind_U1);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldind_U2"/>
        /// </summary>
        public FluentIlGenerator Ldind_U2() {
            IlGenerator.Emit(OpCodes.Ldind_U2);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ldind_U4"/>
        /// </summary>
        public FluentIlGenerator Ldind_U4() {
            IlGenerator.Emit(OpCodes.Ldind_U4);
            return this;
        }
        #endregion
    }
}
