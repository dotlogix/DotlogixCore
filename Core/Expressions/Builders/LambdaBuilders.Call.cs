// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  LambdaBuilders.Call.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 09.06.2021 00:04
// LastEdited:  26.09.2021 22:15
// ==================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Expressions {
    public static partial class LambdaBuilders {
        public static LambdaBuilder<TResult> Call<TResult>(this LambdaBuilder instance, string methodName, params LambdaBuilder[] args) {
            return Call<TResult>(instance, methodName, args.AsEnumerable());
        }

        public static LambdaBuilder<TResult> Call<TResult>(this LambdaBuilder instance, string methodName, IEnumerable<LambdaBuilder> args) {
            args = args.AsReadOnlyCollection();
            return Call<TResult>(instance, ResolveMethod(instance, methodName, args), args);
        }

        public static LambdaBuilder<TResult> Call<TResult>(this LambdaBuilder instance, MethodInfo methodInfo, params LambdaBuilder[] args) {
            return Call<TResult>(instance, methodInfo, args.AsReadOnlyCollection());
        }

        public static LambdaBuilder<TResult> Call<TResult>(this LambdaBuilder instance, MethodInfo methodInfo, IEnumerable<LambdaBuilder> args) {
            args = args.AsReadOnlyCollection();
            var expression = Expression.Call(instance.Body, methodInfo, args.Select(a => a.Body).ToArray());
            return new LambdaBuilder<TResult>(expression);
        }

        public static LambdaBuilder<TResult> Call<TResult>(this LambdaBuilder instance, Func<TResult> method) {
            return Call<TResult>(instance, method.Method);
        }
        public static LambdaBuilder<TResult> Call<T1, TResult>(this LambdaBuilder instance, Func<T1, TResult> method, LambdaBuilder<T1> arg1) {
            return Call<TResult>(instance, method.Method, arg1);
        }
        public static LambdaBuilder<TResult> Call<T1, T2, TResult>(this LambdaBuilder instance, Func<T1, T2, TResult> method, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2) {
            return Call<TResult>(instance, method.Method, arg1, arg2);
        }
        public static LambdaBuilder<TResult> Call<T1, T2, T3, TResult>(this LambdaBuilder instance, Func<T1, T2, T3, TResult> method, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3) {
            return Call<TResult>(instance, method.Method, arg1, arg2, arg3);
        }
        public static LambdaBuilder<TResult> Call<T1, T2, T3, T4, TResult>(this LambdaBuilder instance, Func<T1, T2, T3, T4, TResult> method, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3, LambdaBuilder<T4> arg4) {
            return Call<TResult>(instance, method.Method, arg1, arg2, arg3, arg4);
        }
        public static LambdaBuilder<TResult> Call<T1, T2, T3, T4, T5, TResult>(this LambdaBuilder instance, Func<T1, T2, T3, T4, T5, TResult> method, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3, LambdaBuilder<T4> arg4, LambdaBuilder<T5> arg5) {
            return Call<TResult>(instance, method.Method, arg1, arg2, arg3, arg4, arg5);
        }
        public static LambdaBuilder<TResult> Call<T1, T2, T3, T4, T5, T6, TResult>(this LambdaBuilder instance, Func<T1, T2, T3, T4, T5, T6, TResult> method, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3, LambdaBuilder<T4> arg4, LambdaBuilder<T5> arg5, LambdaBuilder<T6> arg6) {
            return Call<TResult>(instance, method.Method, arg1, arg2, arg3, arg4, arg5, arg6);
        }
        public static LambdaBuilder<TResult> Call<T1, T2, T3, T4, T5, T6, T7, TResult>(this LambdaBuilder instance, Func<T1, T2, T3, T4, T5, T6, T7, TResult> method, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3, LambdaBuilder<T4> arg4, LambdaBuilder<T5> arg5, LambdaBuilder<T6> arg6, LambdaBuilder<T7> arg7) {
            return Call<TResult>(instance, method.Method, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
        public static LambdaBuilder<TResult> Call<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this LambdaBuilder instance, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> method, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3, LambdaBuilder<T4> arg4, LambdaBuilder<T5> arg5, LambdaBuilder<T6> arg6, LambdaBuilder<T7> arg7, LambdaBuilder<T8> arg8) {
            return Call<TResult>(instance, method.Method, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
        public static LambdaBuilder<TResult> Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this LambdaBuilder instance, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> method, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3, LambdaBuilder<T4> arg4, LambdaBuilder<T5> arg5, LambdaBuilder<T6> arg6, LambdaBuilder<T7> arg7, LambdaBuilder<T8> arg8, LambdaBuilder<T9> arg9) {
            return Call<TResult>(instance, method.Method, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }
        public static LambdaBuilder<TResult> Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this LambdaBuilder instance, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> method, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3, LambdaBuilder<T4> arg4, LambdaBuilder<T5> arg5, LambdaBuilder<T6> arg6, LambdaBuilder<T7> arg7, LambdaBuilder<T8> arg8, LambdaBuilder<T9> arg9, LambdaBuilder<T10> arg10) {
            return Call<TResult>(instance, method.Method, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }
    
        #region Helpers      
        private static MethodInfo ResolveMethod(LambdaBuilder instance, string methodName, IEnumerable<LambdaBuilder> lambdaBuilders) {
            var types = lambdaBuilders.Select(b => b.Type).ToArray();
#if NETSTANDARD2_1
            return instance.Type.GetMethod(methodName, 0, BindingFlags.Instance, default, CallingConventions.Any, types, default);
#else
            return instance.Type.GetMethod(methodName, BindingFlags.Instance, default, CallingConventions.Any, types, default);
#endif
        }
        private static MethodInfo ResolveStaticMethod(Type type, string methodName, IEnumerable<LambdaBuilder> lambdaBuilders) {
            var types = lambdaBuilders.Select(b => b.Type).ToArray();
#if NETSTANDARD2_1
            return type.GetMethod(methodName, 0, BindingFlags.Static, default, CallingConventions.Any, types, default);
#else
            return type.GetMethod(methodName, BindingFlags.Static, default, CallingConventions.Any, types, default);
#endif
        }
        #endregion
    }
}