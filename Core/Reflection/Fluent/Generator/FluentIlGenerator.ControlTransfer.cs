// ==================================================
// Copyright 2016(C) , DotLogix
// File:  FluentIlGenerator.ControlTransfer.cs
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
        /// <summary>
        ///     Exits current method and jumps to specified method.
        /// </summary>
        public FluentIlGenerator Jmp(MethodInfo methodInfo) {
            IlGenerator.Emit(OpCodes.Jmp, methodInfo);
            return this;
        }

        /// <summary>
        ///     Implements a jump table.
        /// </summary>
        public FluentIlGenerator Switch(params Label[] labels) {
            IlGenerator.Emit(OpCodes.Switch, labels);
            return this;
        }

        /// <summary>
        ///     Transfers control from the filter clause of an exception back to the Common Language Infrastructure (CLI)
        ///     exception handler.
        /// </summary>
        public FluentIlGenerator Endfilter() {
            IlGenerator.Emit(OpCodes.Endfilter);
            return this;
        }

        /// <summary>
        ///     Transfers control from the fault or finally clause of an exception block back to the Common Language
        ///     Infrastructure (CLI) exception handler.
        /// </summary>
        public FluentIlGenerator Endfinally() {
            IlGenerator.Emit(OpCodes.Endfinally);
            return this;
        }

        /// <summary>
        ///     Exits a protected region of code, unconditionally transferring control to a specific target instruction.
        /// </summary>
        public FluentIlGenerator Leave(Label label) {
            IlGenerator.Emit(OpCodes.Leave, label);
            return this;
        }

        /// <summary>
        ///     Exits a protected region of code, unconditionally transferring control to a target instruction (short form).
        /// </summary>
        public FluentIlGenerator Leave_S(Label label) {
            IlGenerator.Emit(OpCodes.Leave_S, label);
            return this;
        }

        #region Branch
        /// <summary>
        ///     Transfers control to a target instruction if two values are equal.
        /// </summary>
        public FluentIlGenerator Beq(Label label) {
            IlGenerator.Emit(OpCodes.Beq, label);
            return this;
        }

        /// <summary>
        ///     Transfers control to a target instruction (short form) if two values are equal.
        /// </summary>
        public FluentIlGenerator Beq_S(Label label) {
            IlGenerator.Emit(OpCodes.Beq_S, label);
            return this;
        }

        /// <summary>
        ///     Transfers control to a target instruction if the first value is greater than or equal to the second value.
        /// </summary>
        public FluentIlGenerator Bge(Label label) {
            IlGenerator.Emit(OpCodes.Bge, label);
            return this;
        }

        /// <summary>
        ///     Transfers control to a target instruction (short form) if the first value is greater than or equal to the
        ///     second value.
        /// </summary>
        public FluentIlGenerator Bge_S(Label label) {
            IlGenerator.Emit(OpCodes.Bge_S, label);
            return this;
        }

        /// <summary>
        ///     Transfers control to a target instruction if the first value is greater than the second value, when comparing
        ///     unsigned integer values or unordered float values.
        /// </summary>
        public FluentIlGenerator Bge_Un(Label label) {
            IlGenerator.Emit(OpCodes.Bge_Un, label);
            return this;
        }

        /// <summary>
        ///     Transfers control to a target instruction (short form) if the first value is greater than the second value,
        ///     when comparing unsigned integer values or unordered float values.
        /// </summary>
        public FluentIlGenerator Bge_Un_S(Label label) {
            IlGenerator.Emit(OpCodes.Bge_Un_S, label);
            return this;
        }

        /// <summary>
        ///     Transfers control to a target instruction if the first value is greater than the second value.
        /// </summary>
        public FluentIlGenerator Bgt(Label label) {
            IlGenerator.Emit(OpCodes.Bgt, label);
            return this;
        }

        /// <summary>
        ///     Transfers control to a target instruction (short form) if the first value is greater than the second value.
        /// </summary>
        public FluentIlGenerator Bgt_S(Label label) {
            IlGenerator.Emit(OpCodes.Bgt_S, label);
            return this;
        }

        /// <summary>
        ///     Transfers control to a target instruction if the first value is greater than the second value, when comparing
        ///     unsigned integer values or unordered float values.
        /// </summary>
        public FluentIlGenerator Bgt_Un(Label label) {
            IlGenerator.Emit(OpCodes.Bgt_Un, label);
            return this;
        }

        /// <summary>
        ///     Transfers control to a target instruction (short form) if the first value is greater than the second value,
        ///     when comparing unsigned integer values or unordered float values.
        /// </summary>
        public FluentIlGenerator Bgt_Un_S(Label label) {
            IlGenerator.Emit(OpCodes.Bgt_Un_S, label);
            return this;
        }

        /// <summary>
        ///     Transfers control to a target instruction if the first value is less than or equal to the second value.
        /// </summary>
        public FluentIlGenerator Ble(Label label) {
            IlGenerator.Emit(OpCodes.Ble, label);
            return this;
        }

        /// <summary>
        ///     Transfers control to a target instruction (short form) if the first value is less than or equal to the second
        ///     value.
        /// </summary>
        public FluentIlGenerator Ble_S(Label label) {
            IlGenerator.Emit(OpCodes.Ble_S, label);
            return this;
        }

        /// <summary>
        ///     Transfers control to a target instruction if the first value is less than or equal to the second value, when
        ///     comparing unsigned integer values or unordered float values.
        /// </summary>
        public FluentIlGenerator Ble_Un(Label label) {
            IlGenerator.Emit(OpCodes.Ble_Un, label);
            return this;
        }

        /// <summary>
        ///     Transfers control to a target instruction (short form) if the first value is less than or equal to the second
        ///     value, when comparing unsigned integer values or unordered float values.
        /// </summary>
        public FluentIlGenerator Ble_Un_S(Label label) {
            IlGenerator.Emit(OpCodes.Ble_Un_S, label);
            return this;
        }

        /// <summary>
        ///     Transfers control to a target instruction if the first value is less than the second value.
        /// </summary>
        public FluentIlGenerator Blt(Label label) {
            IlGenerator.Emit(OpCodes.Blt, label);
            return this;
        }

        /// <summary>
        ///     Transfers control to a target instruction (short form) if the first value is less than the second value.
        /// </summary>
        public FluentIlGenerator Blt_S(Label label) {
            IlGenerator.Emit(OpCodes.Blt_S, label);
            return this;
        }

        /// <summary>
        ///     Transfers control to a target instruction if the first value is less than the second value, when comparing
        ///     unsigned integer values or unordered float values.
        /// </summary>
        public FluentIlGenerator Blt_Un(Label label) {
            IlGenerator.Emit(OpCodes.Blt_Un, label);
            return this;
        }

        /// <summary>
        ///     Transfers control to a target instruction (short form) if the first value is less than the second value, when
        ///     comparing unsigned integer values or unordered float values.
        /// </summary>
        public FluentIlGenerator Blt_Un_S(Label label) {
            IlGenerator.Emit(OpCodes.Blt_Un_S, label);
            return this;
        }

        /// <summary>
        ///     Transfers control to a target instruction when two unsigned integer values or unordered float values are not
        ///     equal.
        /// </summary>
        public FluentIlGenerator Bne_Un(Label label) {
            IlGenerator.Emit(OpCodes.Bne_Un, label);
            return this;
        }

        /// <summary>
        ///     Transfers control to a target instruction (short form) when two unsigned integer values or unordered float
        ///     values are not equal.
        /// </summary>
        public FluentIlGenerator Bne_Un_S(Label label) {
            IlGenerator.Emit(OpCodes.Bne_Un_S, label);
            return this;
        }

        /// <summary>
        ///     Unconditionally transfers control to a target instruction.
        /// </summary>
        public FluentIlGenerator Br(Label label) {
            IlGenerator.Emit(OpCodes.Br, label);
            return this;
        }

        /// <summary>
        ///     Unconditionally transfers control to a target instruction (short form).
        /// </summary>
        public FluentIlGenerator Br_S(Label label) {
            IlGenerator.Emit(OpCodes.Br_S, label);
            return this;
        }

        /// <summary>
        ///     Signals the Common Language Infrastructure (CLI) to inform the debugger that a break point has been tripped.
        /// </summary>
        public FluentIlGenerator Break() {
            IlGenerator.Emit(OpCodes.Break);
            return this;
        }

        /// <summary>
        ///     Transfers control to a target instruction if value is false, a null reference (Nothing in Visual Basic), or
        ///     zero.
        /// </summary>
        public FluentIlGenerator Brfalse(Label label) {
            IlGenerator.Emit(OpCodes.Brfalse, label);
            return this;
        }

        /// <summary>
        ///     Transfers control to a target instruction if value is false, a null reference, or zero.
        /// </summary>
        public FluentIlGenerator Brfalse_S(Label label) {
            IlGenerator.Emit(OpCodes.Brfalse_S, label);
            return this;
        }

        /// <summary>
        ///     Transfers control to a target instruction if value is true, not null, or non-zero.
        /// </summary>
        public FluentIlGenerator Brtrue(Label label) {
            IlGenerator.Emit(OpCodes.Brtrue, label);
            return this;
        }

        /// <summary>
        ///     Transfers control to a target instruction (short form) if value is true, not null, or non-zero.
        /// </summary>
        public FluentIlGenerator Brtrue_S(Label label) {
            IlGenerator.Emit(OpCodes.Brtrue_S, label);
            return this;
        }
        #endregion
    }
}
