// ==================================================
// Copyright 2016(C) , DotLogix
// File:  FluentIlGenerator.Methods.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
using System.Reflection;
using System.Reflection.Emit;
#endregion

namespace DotLogix.Core.Reflection.Fluent.Generator {
    public partial class FluentIlGenerator {
        #region Values
        /// <summary>
        ///     Constrains the type on which a virtual method call is made.
        /// </summary>
        public FluentIlGenerator Constrained(Type type) {
            IlGenerator.Emit(OpCodes.Constrained, type);
            return this;
        }
        #endregion

        #region Methods
        #region Call
        /// <summary>
        ///     Calls the method indicated by the passed method descriptor.
        /// </summary>
        public FluentIlGenerator Call(MethodInfo methodInfo) {
            IlGenerator.Emit(OpCodes.Call, methodInfo);
            return this;
        }

        /// <summary>
        ///     Calls the method indicated by the passed method descriptor.
        /// </summary>
        public FluentIlGenerator Call(MethodInfo methodInfo, Type[] optionalParameterTypes) {
            IlGenerator.EmitCall(OpCodes.Call, methodInfo, optionalParameterTypes);
            return this;
        }

        /// <summary>
        ///     Calls the method indicated on the evaluation stack (as a pointer to an entry point) with arguments described
        ///     by a calling convention.
        /// </summary>
        public FluentIlGenerator Calli(CallingConventions callingConvention, Type returnType, Type[] parameterTypes,
                                       Type[] optionalParameterTypes) {
            IlGenerator.EmitCalli(OpCodes.Calli, callingConvention, returnType, parameterTypes, optionalParameterTypes);
            return this;
        }

        /// <summary>
        ///     Calls a late-bound method on an object, pushing the return value onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Callvirt(MethodInfo methodInfo) {
            IlGenerator.Emit(OpCodes.Call, methodInfo);
            return this;
        }

        /// <summary>
        ///     Calls a late-bound method on an object, pushing the return value onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Callvirt(MethodInfo methodInfo, Type[] optionalParameterTypes) {
            IlGenerator.EmitCall(OpCodes.Call, methodInfo, optionalParameterTypes);
            return this;
        }
        #endregion

        #region Load Arg
        /// <summary>
        ///     Loads an argument (referenced by a specified index value) onto the stack.
        /// </summary>
        public FluentIlGenerator Ldarg(short index) {
            IlGenerator.Emit(OpCodes.Ldarg, index);
            return this;
        }

        /// <summary>
        ///     Loads the argument at index 0 onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Ldarg_0() {
            IlGenerator.Emit(OpCodes.Ldarg_0);
            return this;
        }

        /// <summary>
        ///     Loads the argument at index 1 onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Ldarg_1() {
            IlGenerator.Emit(OpCodes.Ldarg_1);
            return this;
        }

        /// <summary>
        ///     Loads the argument at index 2 onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Ldarg_2() {
            IlGenerator.Emit(OpCodes.Ldarg_2);
            return this;
        }

        /// <summary>
        ///     Loads the argument at index 3 onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Ldarg_3() {
            IlGenerator.Emit(OpCodes.Ldarg_3);
            return this;
        }

        /// <summary>
        ///     Loads the argument (referenced by a specified short form index) onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Ldarg_S(byte index) {
            IlGenerator.Emit(OpCodes.Ldarg_S, index);
            return this;
        }

        /// <summary>
        ///     Load an argument address onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Ldarga(short index) {
            IlGenerator.Emit(OpCodes.Ldarga, index);
            return this;
        }

        /// <summary>
        ///     Load an argument address, in short form, onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Ldarga_S(byte index) {
            IlGenerator.Emit(OpCodes.Ldarga_S, index);
            return this;
        }
        #endregion

        #region Store Arg
        /// <summary>
        ///     Stores the value on top of the evaluation stack in the argument slot at a specified index, short form.
        /// </summary>
        public FluentIlGenerator Starg_S(byte index) {
            IlGenerator.Emit(OpCodes.Starg_S, index);
            return this;
        }

        /// <summary>
        ///     Stores the value on top of the evaluation stack in the argument slot at a specified index.
        /// </summary>
        public FluentIlGenerator Starg(short index) {
            IlGenerator.Emit(OpCodes.Starg, index);
            return this;
        }
        #endregion

        /// <summary>
        ///     Returns an unmanaged pointer to the argument list of the current method.
        /// </summary>
        public FluentIlGenerator Arglist() {
            IlGenerator.Emit(OpCodes.Arglist);
            return this;
        }

        /// <summary>
        ///     Returns from the current method, pushing a return value (if present) from the callee's evaluation stack onto
        ///     the caller's evaluation stack.
        /// </summary>
        public FluentIlGenerator Ret() {
            IlGenerator.Emit(OpCodes.Ret);
            return this;
        }
        #endregion
    }
}
