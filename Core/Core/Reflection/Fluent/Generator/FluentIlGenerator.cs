// ==================================================
// Copyright 2018(C) , DotLogix
// File:  FluentIlGenerator.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Reflection;
using System.Reflection.Emit;
#pragma warning disable 1591
#endregion

namespace DotLogix.Core.Reflection.Fluent.Generator {
    public partial class FluentIlGenerator {
        /// <summary>
        ///     Get the IlGenerator
        /// </summary>
        public ILGenerator IlGenerator { get; }

        /// <summary>
        /// Creates a new instance of <see cref="FluentIlGenerator"/>
        /// </summary>
        /// <param name="ilGenerator"></param>
        public FluentIlGenerator(ILGenerator ilGenerator) {
            IlGenerator = ilGenerator ?? throw new ArgumentNullException(nameof(ilGenerator));
        }


        /// <summary>
        ///     Copies a specified number bytes from a source address to a destination address.
        /// </summary>
        public FluentIlGenerator Cpblk() {
            IlGenerator.Emit(OpCodes.Cpblk);
            return this;
        }

        /// <summary>
        ///     Copies the value type located at the address of an object (type &amp;, * or native int) to the address of the
        ///     destination object (type &amp;, * or native int).
        /// </summary>
        public FluentIlGenerator Cpobj(Type type) {
            IlGenerator.Emit(OpCodes.Cpobj, type);
            return this;
        }

        /// <summary>
        ///     Copies the current topmost value on the evaluation stack, and then pushes the copy onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Dup() {
            IlGenerator.Emit(OpCodes.Dup);
            return this;
        }

        /// <summary>
        ///     Initializes a specified block of memory at a specific address to a given size and initial value.
        /// </summary>
        public FluentIlGenerator Initblk() {
            IlGenerator.Emit(OpCodes.Initblk);
            return this;
        }

        /// <summary>
        ///     Allocates a certain number of bytes from the local dynamic memory pool and pushes the address (a transient
        ///     pointer, type *) of the first allocated byte onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Localloc() {
            IlGenerator.Emit(OpCodes.Localloc);
            return this;
        }

        /// <summary>
        ///     Rethrows the current exception.
        /// </summary>
        public FluentIlGenerator Rethrow() {
            IlGenerator.Emit(OpCodes.Rethrow);
            return this;
        }

        /// <summary>
        ///     Throws the exception object currently on the evaluation stack.
        /// </summary>
        public FluentIlGenerator Throw() {
            IlGenerator.Emit(OpCodes.Throw);
            return this;
        }

        /// <summary>
        ///     Removes the value currently on top of the evaluation stack.
        /// </summary>
        public FluentIlGenerator Pop() {
            IlGenerator.Emit(OpCodes.Pop);
            return this;
        }

        /// <summary>
        ///     Specifies that the subsequent array address operation performs no type check at run time, and that it returns
        ///     a managed pointer whose mutability is restricted.
        /// </summary>
        public FluentIlGenerator Readonly() {
            IlGenerator.Emit(OpCodes.Readonly);
            return this;
        }

        /// <summary>
        ///     Specifies that an address currently atop the evaluation stack might be volatile, and the results of reading
        ///     that location cannot be cached or that multiple stores to that location cannot be suppressed.
        /// </summary>
        public FluentIlGenerator Volatile() {
            IlGenerator.Emit(OpCodes.Volatile);
            return this;
        }

        /// <summary>
        ///     Performs a postfixed method call instruction such that the current method's stack frame is removed before the
        ///     actual call instruction is executed.
        /// </summary>
        public FluentIlGenerator Tailcall() {
            IlGenerator.Emit(OpCodes.Tailcall);
            return this;
        }

        /// <summary>
        ///     Indicates that an address currently atop the evaluation stack might not be aligned to the natural size of the
        ///     immediately following ldind, stind, ldfld, stfld, ldobj, stobj, initblk, or cpblk instruction.
        /// </summary>
        public FluentIlGenerator Unaligned(Label label) {
            IlGenerator.Emit(OpCodes.Unaligned, label);
            return this;
        }

        /// <summary>
        ///     Indicates that an address currently atop the evaluation stack might not be aligned to the natural size of the
        ///     immediately following ldind, stind, ldfld, stfld, ldobj, stobj, initblk, or cpblk instruction.
        /// </summary>
        public FluentIlGenerator Unaligned(byte alignment) {
            IlGenerator.Emit(OpCodes.Unaligned, alignment);
            return this;
        }

        /// <summary>
        ///     Pushes the size, in bytes, of a supplied value type onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Sizeof(Type type) {
            IlGenerator.Emit(OpCodes.Sizeof, type);
            return this;
        }

        /// <summary>
        ///     Fills space if opcodes are patched. No meaningful operation is performed although a processing cycle can be
        ///     consumed.
        /// </summary>
        public FluentIlGenerator Nop() {
            IlGenerator.Emit(OpCodes.Nop);
            return this;
        }

        /// <summary>
        ///     Converts a metadata token to its runtime representation, pushing it onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Ldtoken(MethodInfo methodInfo) {
            IlGenerator.Emit(OpCodes.Ldtoken, methodInfo);
            return this;
        }

        /// <summary>
        ///     Converts a metadata token to its runtime representation, pushing it onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Ldtoken(FieldInfo fieldInfo) {
            IlGenerator.Emit(OpCodes.Ldtoken, fieldInfo);
            return this;
        }

        /// <summary>
        ///     Converts a metadata token to its runtime representation, pushing it onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Ldtoken(Type type) {
            IlGenerator.Emit(OpCodes.Ldtoken, type);
            return this;
        }

        /// <summary>
        ///     Pushes an unmanaged pointer (type native int) to the native code implementing a specific method onto the
        ///     evaluation stack.
        /// </summary>
        public FluentIlGenerator Ldftn(MethodInfo methodInfo) {
            IlGenerator.Emit(OpCodes.Ldftn, methodInfo);
            return this;
        }

        /// <summary>
        ///     Pushes an unmanaged pointer (type native int) to the native code implementing a particular virtual method
        ///     associated with a specified object onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Ldvirtftn(MethodInfo methodInfo) {
            IlGenerator.Emit(OpCodes.Ldvirtftn, methodInfo);
            return this;
        }

        /// <summary>
        ///     Pushes a typed reference to a new instance of a specific type onto the evaluation stack.
        /// </summary>
        public FluentIlGenerator Mkrefany(Type type) {
            IlGenerator.Emit(OpCodes.Mkrefany, type);
            return this;
        }

        /// <summary>
        ///     Retrieves the type token embedded in a typed reference.
        /// </summary>
        public FluentIlGenerator Refanytype() {
            IlGenerator.Emit(OpCodes.Refanytype);
            return this;
        }

        /// <summary>
        ///     Retrieves the address (type &amp;) embedded in a typed reference.
        /// </summary>
        public FluentIlGenerator Refanyval(Type type) {
            IlGenerator.Emit(OpCodes.Refanyval, type);
            return this;
        }

        #region Wrapper
        public Label BeginExceptionBlock() {
            return IlGenerator.BeginExceptionBlock();
        }

        public FluentIlGenerator BeginExceptionBlock(out Label label) {
            label = IlGenerator.BeginExceptionBlock();
            return this;
        }

        public FluentIlGenerator EndExceptionBlock() {
            IlGenerator.EndExceptionBlock();
            return this;
        }

        public FluentIlGenerator BeginExceptFilterBlock() {
            IlGenerator.BeginExceptFilterBlock();
            return this;
        }

        public FluentIlGenerator BeginCatchBlock(Type type) {
            IlGenerator.BeginCatchBlock(type);
            return this;
        }

        public FluentIlGenerator BeginFaultBlock() {
            IlGenerator.BeginFaultBlock();
            return this;
        }

        public FluentIlGenerator BeginFinallyBlock() {
            IlGenerator.BeginFinallyBlock();
            return this;
        }

        public Label DefineLabel() {
            return IlGenerator.DefineLabel();
        }

        public FluentIlGenerator DefineLabel(out Label label) {
            label = IlGenerator.DefineLabel();
            return this;
        }

        public FluentIlGenerator MarkLabel(Label label) {
            IlGenerator.MarkLabel(label);
            return this;
        }

        public FluentIlGenerator ThrowException(Type type) {
            IlGenerator.ThrowException(type);
            return this;
        }

        public FluentIlGenerator EmitWriteLine(string value) {
            IlGenerator.EmitWriteLine(value);
            return this;
        }

        public FluentIlGenerator EmitWriteLine(LocalBuilder localBuilder) {
            IlGenerator.EmitWriteLine(localBuilder);
            return this;
        }

        public FluentIlGenerator EmitWriteLine(FieldInfo fieldInfo) {
            IlGenerator.EmitWriteLine(fieldInfo);
            return this;
        }

        public LocalBuilder DeclareLocal(Type type) {
            return IlGenerator.DeclareLocal(type);
        }

        public FluentIlGenerator DeclareLocal(Type type, out LocalBuilder localBuilder) {
            localBuilder = IlGenerator.DeclareLocal(type);
            return this;
        }

        public LocalBuilder DeclareLocal(Type type, bool pinned) {
            return IlGenerator.DeclareLocal(type, pinned);
        }

        public FluentIlGenerator DeclareLocal(Type type, bool pinned, out LocalBuilder localBuilder) {
            localBuilder = IlGenerator.DeclareLocal(type, pinned);
            return this;
        }

        public FluentIlGenerator UsingNamespace(string usingNamespace) {
            IlGenerator.UsingNamespace(usingNamespace);
            return this;
        }

        public FluentIlGenerator BeginScope() {
            IlGenerator.BeginScope();
            return this;
        }

        public FluentIlGenerator EndScope() {
            IlGenerator.EndScope();
            return this;
        }
        #endregion
    }
}