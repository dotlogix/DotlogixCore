// ==================================================
// Copyright 2016(C) , DotLogix
// File:  FluentIlGenerator.Fields.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System.Reflection;
using System.Reflection.Emit;
#endregion

namespace DotLogix.Core.Reflection.Fluent.Generator {
    public partial class FluentIlGenerator {
        #region Load
        /// <summary>
        ///     Finds the value of a field in the object whose reference is currently on the evaluation stack.
        /// </summary>
        public FluentIlGenerator Ldfld(FieldInfo fieldInfo) {
            IlGenerator.Emit(OpCodes.Ldfld, fieldInfo);
            return this;
        }

        /// <summary>
        ///     Finds the address of a field in the object whose reference is currently on the evaluation stack.
        /// </summary>
        public FluentIlGenerator Ldflda(FieldInfo fieldInfo) {
            IlGenerator.Emit(OpCodes.Ldflda, fieldInfo);
            return this;
        }

        /// <summary>
        ///     Pushes the value of a static field onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Ldsfld(FieldInfo fieldInfo) {
            IlGenerator.Emit(OpCodes.Ldsfld, fieldInfo);
            return this;
        }

        /// <summary>
        ///     Pushes the address of a static field onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Ldsflda(FieldInfo fieldInfo) {
            IlGenerator.Emit(OpCodes.Ldsflda, fieldInfo);
            return this;
        }
        #endregion

        #region Store
        /// <summary>
        ///     Replaces the value stored in the field of an object reference or pointer with a new value.
        /// </summary>
        public FluentIlGenerator Stfld(FieldInfo fieldInfo) {
            IlGenerator.Emit(OpCodes.Stfld, fieldInfo);
            return this;
        }

        /// <summary>
        ///     Replaces the value of a static field with a value from the evaluation stack.
        /// </summary>
        public FluentIlGenerator Stsfld(FieldInfo fieldInfo) {
            IlGenerator.Emit(OpCodes.Stsfld, fieldInfo);
            return this;
        }
        #endregion
    }
}
