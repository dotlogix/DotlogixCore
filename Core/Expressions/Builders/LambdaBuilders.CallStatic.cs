// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  LambdaBuilders.CallStatic.cs
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
        public static LambdaBuilder<TResult> CallStatic<TResult>(Type type, string methodName, params LambdaBuilder[] args) {
            return CallStatic<TResult>(type, methodName, args.AsEnumerable());
        }

        public static LambdaBuilder<TResult> CallStatic<TResult>(Type type, string methodName, IEnumerable<LambdaBuilder> args) {
            args = args.AsReadOnlyCollection();
            return CallStatic<TResult>(ResolveStaticMethod(type, methodName, args), args);
        }

        public static LambdaBuilder<TResult> CallStatic<TResult>(MethodInfo methodInfo, params LambdaBuilder[] args) {
            return CallStatic<TResult>(methodInfo, args.AsEnumerable());
        }

        public static LambdaBuilder<TResult> CallStatic<TResult>(MethodInfo methodInfo, IEnumerable<LambdaBuilder> args) {
            if(methodInfo.ReturnType != typeof(TResult)) {
                throw new ArgumentException($"Expected method return type {typeof(TResult).Name}, but got {methodInfo.ReturnType.Name}");
            }

            var parameters = methodInfo.GetParameters();
            var arguments = new Expression[parameters.Length];
            var idx = 0;
            foreach(var arg in args) {
                var parameterType = parameters[idx].ParameterType;
                var argument = arg.Cast(parameterType);
                arguments[idx] = argument.Body;
                idx++;
            }

            if(idx < parameters.Length && parameters[idx].IsOptional == false) {
                throw new ArgumentException($"Not enough parameters. Expected {parameters.Length}, but got {idx}");
            }

            return Expression.Call(methodInfo, arguments);
        }

        public static LambdaBuilder<TResult> CallStatic<TResult>(Func<TResult> methodFunc) {
            return CallStatic<TResult>(methodFunc.Method);
        }

        public static LambdaBuilder<TResult> CallStatic<T1, TResult>(Func<T1, TResult> methodFunc, LambdaBuilder<T1> arg1) {
            return CallStatic<TResult>(methodFunc.Method, arg1);
        }
        public static LambdaBuilder<TResult> CallStatic<T1, T2, TResult>(Func<T1, T2, TResult> methodFunc, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2) {
            return CallStatic<TResult>(methodFunc.Method, arg1, arg2);
        }
        public static LambdaBuilder<TResult> CallStatic<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> methodFunc, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3) {
            return CallStatic<TResult>(methodFunc.Method, arg1, arg2, arg3);
        }
        public static LambdaBuilder<TResult> CallStatic<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> methodFunc, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3, LambdaBuilder<T4> arg4) {
            return CallStatic<TResult>(methodFunc.Method, arg1, arg2, arg3, arg4);
        }
        public static LambdaBuilder<TResult> CallStatic<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> methodFunc, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3, LambdaBuilder<T4> arg4, LambdaBuilder<T5> arg5) {
            return CallStatic<TResult>(methodFunc.Method, arg1, arg2, arg3, arg4, arg5);
        }
        public static LambdaBuilder<TResult> CallStatic<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> methodFunc, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3, LambdaBuilder<T4> arg4, LambdaBuilder<T5> arg5, LambdaBuilder<T6> arg6) {
            return CallStatic<TResult>(methodFunc.Method, arg1, arg2, arg3, arg4, arg5, arg6);
        }
        public static LambdaBuilder<TResult> CallStatic<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> methodFunc, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3, LambdaBuilder<T4> arg4, LambdaBuilder<T5> arg5, LambdaBuilder<T6> arg6, LambdaBuilder<T7> arg7) {
            return CallStatic<TResult>(methodFunc.Method, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
        public static LambdaBuilder<TResult> CallStatic<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> methodFunc, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3, LambdaBuilder<T4> arg4, LambdaBuilder<T5> arg5, LambdaBuilder<T6> arg6, LambdaBuilder<T7> arg7, LambdaBuilder<T8> arg8) {
            return CallStatic<TResult>(methodFunc.Method, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
        public static LambdaBuilder<TResult> CallStatic<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> methodFunc, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3, LambdaBuilder<T4> arg4, LambdaBuilder<T5> arg5, LambdaBuilder<T6> arg6, LambdaBuilder<T7> arg7, LambdaBuilder<T8> arg8, LambdaBuilder<T9> arg9) {
            return CallStatic<TResult>(methodFunc.Method, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }
        public static LambdaBuilder<TResult> CallStatic<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> methodFunc, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3, LambdaBuilder<T4> arg4, LambdaBuilder<T5> arg5, LambdaBuilder<T6> arg6, LambdaBuilder<T7> arg7, LambdaBuilder<T8> arg8, LambdaBuilder<T9> arg9, LambdaBuilder<T10> arg10) {
            return CallStatic<TResult>(methodFunc.Method, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }
    }
}