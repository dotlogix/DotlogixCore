// ==================================================
// Copyright 2018(C) , DotLogix
// File:  FluentIlGenerator.Indirect.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Reflection.Emit;
#endregion

namespace DotLogix.Core.Reflection.Fluent.Generator {
    public partial class FluentIlGenerator {
        #region Store
        /// <summary>
        ///     Stores a value of type native int at a supplied address.
        /// </summary>
        public FluentIlGenerator Stind_I() {
            IlGenerator.Emit(OpCodes.Stind_I);
            return this;
        }

        /// <summary>
        ///     Stores a value of type int8 at a supplied address.
        /// </summary>
        public FluentIlGenerator Stind_I1() {
            IlGenerator.Emit(OpCodes.Stind_I1);
            return this;
        }

        /// <summary>
        ///     Stores a value of type int16 at a supplied address.
        /// </summary>
        public FluentIlGenerator Stind_I2() {
            IlGenerator.Emit(OpCodes.Stind_I2);
            return this;
        }

        /// <summary>
        ///     Stores a value of type int32 at a supplied address.
        /// </summary>
        public FluentIlGenerator Stind_I4() {
            IlGenerator.Emit(OpCodes.Stind_I4);
            return this;
        }

        /// <summary>
        ///     Stores a value of type int64 at a supplied address.
        /// </summary>
        public FluentIlGenerator Stind_I8() {
            IlGenerator.Emit(OpCodes.Stind_I8);
            return this;
        }

        /// <summary>
        ///     Stores a value of type float32 at a supplied address.
        /// </summary>
        public FluentIlGenerator Stind_R4() {
            IlGenerator.Emit(OpCodes.Stind_R4);
            return this;
        }

        /// <summary>
        ///     Stores a value of type float64 at a supplied address.
        /// </summary>
        public FluentIlGenerator Stind_R8() {
            IlGenerator.Emit(OpCodes.Stind_R8);
            return this;
        }

        /// <summary>
        ///     Stores a object reference value at a supplied address.
        /// </summary>
        public FluentIlGenerator Stind_Ref() {
            IlGenerator.Emit(OpCodes.Stind_Ref);
            return this;
        }
        #endregion

        #region Load
        /// <summary>
        ///     Loads a value of type native int as a native int onto the evaluation stack indirectly.
        /// </summary>
        public FluentIlGenerator Ldind_I() {
            IlGenerator.Emit(OpCodes.Ldind_I);
            return this;
        }

        /// <summary>
        ///     Loads a value of type int8 as an int32 onto the evaluation stack indirectly.
        /// </summary>
        public FluentIlGenerator Ldind_I1() {
            IlGenerator.Emit(OpCodes.Ldind_I1);
            return this;
        }

        /// <summary>
        ///     Loads a value of type int16 as an int32 onto the evaluation stack indirectly.
        /// </summary>
        public FluentIlGenerator Ldind_I2() {
            IlGenerator.Emit(OpCodes.Ldind_I2);
            return this;
        }

        /// <summary>
        ///     Loads a value of type int32 as an int32 onto the evaluation stack indirectly.
        /// </summary>
        public FluentIlGenerator Ldind_I4() {
            IlGenerator.Emit(OpCodes.Ldind_I4);
            return this;
        }

        /// <summary>
        ///     Loads a value of type int64 as an int64 onto the evaluation stack indirectly.
        /// </summary>
        public FluentIlGenerator Ldind_I8() {
            IlGenerator.Emit(OpCodes.Ldind_I8);
            return this;
        }

        /// <summary>
        ///     Loads a value of type float32 as a type F (float) onto the evaluation stack indirectly.
        /// </summary>
        public FluentIlGenerator Ldind_R4() {
            IlGenerator.Emit(OpCodes.Ldind_R4);
            return this;
        }

        /// <summary>
        ///     Loads a value of type float64 as a type F (float) onto the evaluation stack indirectly.
        /// </summary>
        public FluentIlGenerator Ldind_R8() {
            IlGenerator.Emit(OpCodes.Ldind_R8);
            return this;
        }

        /// <summary>
        ///     Loads an object reference as a type O (object reference) onto the evaluation stack indirectly.
        /// </summary>
        public FluentIlGenerator Ldind_Ref() {
            IlGenerator.Emit(OpCodes.Ldind_Ref);
            return this;
        }

        /// <summary>
        ///     Loads a value of type unsigned int8 as an int32 onto the evaluation stack indirectly.
        /// </summary>
        public FluentIlGenerator Ldind_U1() {
            IlGenerator.Emit(OpCodes.Ldind_U1);
            return this;
        }

        /// <summary>
        ///     Loads a value of type unsigned int16 as an int32 onto the evaluation stack indirectly.
        /// </summary>
        public FluentIlGenerator Ldind_U2() {
            IlGenerator.Emit(OpCodes.Ldind_U2);
            return this;
        }

        /// <summary>
        ///     Loads a value of type unsigned int32 as an int32 onto the evaluation stack indirectly.
        /// </summary>
        public FluentIlGenerator Ldind_U4() {
            IlGenerator.Emit(OpCodes.Ldind_U4);
            return this;
        }
        #endregion
    }
}
