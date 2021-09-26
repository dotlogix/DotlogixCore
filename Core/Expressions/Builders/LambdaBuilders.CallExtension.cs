// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  LambdaBuilders.CallExtension.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 09.06.2021 00:04
// LastEdited:  26.09.2021 22:15
// ==================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotLogix.Core.Expressions {
    public static partial class LambdaBuilders {
        public static LambdaBuilder<TResult> CallStatic<TResult>(this LambdaBuilder instance, Type type, string methodName, params LambdaBuilder[] args) {
            return CallStatic<TResult>(type, methodName, args.Prepend(instance));
        }

        public static LambdaBuilder<TResult> CallStatic<TResult>(this LambdaBuilder instance, Type type, string methodName, IEnumerable<LambdaBuilder> args) {
            return CallStatic<TResult>(type, methodName, args.Prepend(instance));
        }

        public static LambdaBuilder<TResult> CallStatic<TResult>(this LambdaBuilder instance, MethodInfo methodInfo, params LambdaBuilder[] args) {
            return CallStatic<TResult>(methodInfo, args.Prepend(instance));
        }

        public static LambdaBuilder<TResult> CallStatic<TResult>(this LambdaBuilder instance, MethodInfo methodInfo, IEnumerable<LambdaBuilder> args) {
            return CallStatic<TResult>(methodInfo, args.Prepend(instance));
        }

        public static LambdaBuilder<TResult> CallStatic<T, TResult>(this LambdaBuilder<T> instance, Func<T, TResult> method) {
            return CallStatic<TResult>(method.Method, instance);
        }
        public static LambdaBuilder<TResult> CallStatic<T, T1, TResult>(this LambdaBuilder<T> instance, Func<T, T1, TResult> method, LambdaBuilder<T1> arg1) {
            return CallStatic<TResult>(method.Method, instance, arg1);
        }
        public static LambdaBuilder<TResult> CallStatic<T, T1, T2, TResult>(this LambdaBuilder<T> instance, Func<T, T1, T2, TResult> method, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2) {
            return CallStatic<TResult>(method.Method, instance, arg1, arg2);
        }
        public static LambdaBuilder<TResult> CallStatic<T, T1, T2, T3, TResult>(this LambdaBuilder<T> instance, Func<T, T1, T2, T3, TResult> method, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3) {
            return CallStatic<TResult>(method.Method, instance, arg1, arg2, arg3);
        }
        public static LambdaBuilder<TResult> CallStatic<T, T1, T2, T3, T4, TResult>(this LambdaBuilder<T> instance, Func<T, T1, T2, T3, T4, TResult> method, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3, LambdaBuilder<T4> arg4) {
            return CallStatic<TResult>(method.Method, instance, arg1, arg2, arg3, arg4);
        }
        public static LambdaBuilder<TResult> CallStatic<T, T1, T2, T3, T4, T5, TResult>(this LambdaBuilder<T> instance, Func<T, T1, T2, T3, T4, T5, TResult> method, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3, LambdaBuilder<T4> arg4, LambdaBuilder<T5> arg5) {
            return CallStatic<TResult>(method.Method, instance, arg1, arg2, arg3, arg4, arg5);
        }
        public static LambdaBuilder<TResult> CallStatic<T, T1, T2, T3, T4, T5, T6, TResult>(this LambdaBuilder<T> instance, Func<T, T1, T2, T3, T4, T5, T6, TResult> method, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3, LambdaBuilder<T4> arg4, LambdaBuilder<T5> arg5, LambdaBuilder<T6> arg6) {
            return CallStatic<TResult>(method.Method, instance, arg1, arg2, arg3, arg4, arg5, arg6);
        }
        public static LambdaBuilder<TResult> CallStatic<T, T1, T2, T3, T4, T5, T6, T7, TResult>(this LambdaBuilder<T> instance, Func<T, T1, T2, T3, T4, T5, T6, T7, TResult> method, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3, LambdaBuilder<T4> arg4, LambdaBuilder<T5> arg5, LambdaBuilder<T6> arg6, LambdaBuilder<T7> arg7) {
            return CallStatic<TResult>(method.Method, instance, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
        public static LambdaBuilder<TResult> CallStatic<T, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this LambdaBuilder<T> instance, Func<T, T1, T2, T3, T4, T5, T6, T7, T8, TResult> method, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3, LambdaBuilder<T4> arg4, LambdaBuilder<T5> arg5, LambdaBuilder<T6> arg6, LambdaBuilder<T7> arg7, LambdaBuilder<T8> arg8) {
            return CallStatic<TResult>(method.Method, instance, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
        public static LambdaBuilder<TResult> CallStatic<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this LambdaBuilder<T> instance, Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> method, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3, LambdaBuilder<T4> arg4, LambdaBuilder<T5> arg5, LambdaBuilder<T6> arg6, LambdaBuilder<T7> arg7, LambdaBuilder<T8> arg8, LambdaBuilder<T9> arg9) {
            return CallStatic<TResult>(method.Method, instance, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }
        public static LambdaBuilder<TResult> CallStatic<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this LambdaBuilder<T> instance, Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> method, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3, LambdaBuilder<T4> arg4, LambdaBuilder<T5> arg5, LambdaBuilder<T6> arg6, LambdaBuilder<T7> arg7, LambdaBuilder<T8> arg8, LambdaBuilder<T9> arg9, LambdaBuilder<T10> arg10) {
            return CallStatic<TResult>(method.Method, instance, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }
    }
}