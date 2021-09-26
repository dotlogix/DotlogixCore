// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  LambdaBuilders.Lambda.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 09.06.2021 00:05
// LastEdited:  26.09.2021 22:15
// ==================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Expressions {
    public static partial class LambdaBuilders {
        public static Expression<TDelegate> ToLambda<TDelegate>(this LambdaBuilder instance, params ParameterExpression[] parameters) where TDelegate : Delegate {
            return ToLambda<TDelegate>(instance, parameters.AsEnumerable());
        }

        public static Expression<TDelegate> ToLambda<TDelegate>(this LambdaBuilder instance, IEnumerable<ParameterExpression> parameters) where TDelegate : Delegate {
            var arguments = typeof(TDelegate).GetGenericArguments();
            var parameterList = parameters.AsReadOnlyList();

            if(arguments.Length - 1 != parameterList.Count) {
                throw new ArgumentException($"Invalid amount of parameters, expected {arguments.Length} but got {parameterList.Count}", nameof(parameters));
            }
            
            return Expression.Lambda<TDelegate>(instance, parameterList);
        }

        public static Expression<Func<TResult>> Lambda<TResult>(this LambdaBuilder<TResult> instance) {
            return ToLambda<Func<TResult>>(instance);
        }
        public static Expression<Func<T1, TResult>> Lambda<T1, TResult>(this LambdaBuilder<TResult> instance, ParameterExpression arg1) {
            return ToLambda<Func<T1, TResult>>(instance, arg1);
        }
        public static Expression<Func<T1, T2, TResult>> Lambda<T1, T2, TResult>(this LambdaBuilder<TResult> instance, ParameterExpression arg1, ParameterExpression arg2) {
            return ToLambda<Func<T1, T2, TResult>>(instance, arg1, arg2);
        }
        public static Expression<Func<T1, T2, T3, TResult>> Lambda<T1, T2, T3, TResult>(this LambdaBuilder<TResult> instance, ParameterExpression arg1, ParameterExpression arg2, ParameterExpression arg3) {
            return ToLambda<Func<T1, T2, T3, TResult>>(instance, arg1, arg2, arg3);
        }
        public static Expression<Func<T1, T2, T3, T4, TResult>> Lambda<T1, T2, T3, T4, TResult>(this LambdaBuilder<TResult> instance, ParameterExpression arg1, ParameterExpression arg2, ParameterExpression arg3, ParameterExpression arg4) {
            return ToLambda<Func<T1, T2, T3, T4, TResult>>(instance, arg1, arg2, arg3, arg4);
        }
        public static Expression<Func<T1, T2, T3, T4, T5, TResult>> Lambda<T1, T2, T3, T4, T5, TResult>(this LambdaBuilder<TResult> instance, ParameterExpression arg1, ParameterExpression arg2, ParameterExpression arg3, ParameterExpression arg4, ParameterExpression arg5) {
            return ToLambda<Func<T1, T2, T3, T4, T5, TResult>>(instance, arg1, arg2, arg3, arg4, arg5);
        }
        public static Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> Lambda<T1, T2, T3, T4, T5, T6, TResult>(this LambdaBuilder<TResult> instance, ParameterExpression arg1, ParameterExpression arg2, ParameterExpression arg3, ParameterExpression arg4, ParameterExpression arg5, ParameterExpression arg6) {
            return ToLambda<Func<T1, T2, T3, T4, T5, T6, TResult>>(instance, arg1, arg2, arg3, arg4, arg5, arg6);
        }
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, TResult>> Lambda<T1, T2, T3, T4, T5, T6, T7, TResult>(this LambdaBuilder<TResult> instance, ParameterExpression arg1, ParameterExpression arg2, ParameterExpression arg3, ParameterExpression arg4, ParameterExpression arg5, ParameterExpression arg6, ParameterExpression arg7) {
            return ToLambda<Func<T1, T2, T3, T4, T5, T6, T7, TResult>>(instance, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> Lambda<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this LambdaBuilder<TResult> instance, ParameterExpression arg1, ParameterExpression arg2, ParameterExpression arg3, ParameterExpression arg4, ParameterExpression arg5, ParameterExpression arg6, ParameterExpression arg7, ParameterExpression arg8) {
            return ToLambda<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>>(instance, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> Lambda<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this LambdaBuilder<TResult> instance, ParameterExpression arg1, ParameterExpression arg2, ParameterExpression arg3, ParameterExpression arg4, ParameterExpression arg5, ParameterExpression arg6, ParameterExpression arg7, ParameterExpression arg8, ParameterExpression arg9) {
            return ToLambda<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>>(instance, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> Lambda<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this LambdaBuilder<TResult> instance, ParameterExpression arg1, ParameterExpression arg2, ParameterExpression arg3, ParameterExpression arg4, ParameterExpression arg5, ParameterExpression arg6, ParameterExpression arg7, ParameterExpression arg8, ParameterExpression arg9, ParameterExpression arg10) {
            return ToLambda<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>>(instance, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }
    }
}