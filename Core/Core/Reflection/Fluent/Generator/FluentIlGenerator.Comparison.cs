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
        ///     <inheritdoc cref="OpCodes.Ceq"/>
        /// </summary>
        public FluentIlGenerator Ceq() {
            IlGenerator.Emit(OpCodes.Ceq);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Cgt"/>
        /// </summary>
        public FluentIlGenerator Cgt() {
            IlGenerator.Emit(OpCodes.Cgt);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Cgt_Un"/>
        /// </summary>
        public FluentIlGenerator Cgt_Un() {
            IlGenerator.Emit(OpCodes.Cgt_Un);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Clt"/>
        /// </summary>
        public FluentIlGenerator Clt() {
            IlGenerator.Emit(OpCodes.Clt);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Clt_Un"/>
        /// </summary>
        public FluentIlGenerator Clt_Un() {
            IlGenerator.Emit(OpCodes.Clt_Un);
            return this;
        }

        /// <summary>
        ///     <inheritdoc cref="OpCodes.Ckfinite"/>
        /// </summary>
        public FluentIlGenerator Ckfinite() {
            IlGenerator.Emit(OpCodes.Ckfinite);
            return this;
        }
    }
}
