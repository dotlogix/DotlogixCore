using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Expressions {
    public static partial class Lambdas {
#region ToLambda
        public static Expression<TDelegate> ToLambda<TDelegate>(this Lambda instance, params ParameterExpression[] parameters) where TDelegate : Delegate {
            return ToLambda<TDelegate>(instance, parameters.AsEnumerable());
        }

        public static Expression<TDelegate> ToLambda<TDelegate>(this Lambda instance, IEnumerable<ParameterExpression> parameters) where TDelegate : Delegate {
            var arguments = typeof(TDelegate).GetGenericArguments();
            var parameterList = parameters.AsReadOnlyList();

            if(arguments.Length - 1 != parameterList.Count) {
                throw new ArgumentException($"Invalid amount of parameters, expected {arguments.Length} but got {parameterList.Count}", nameof(parameters));
            }
            
            return Expression.Lambda<TDelegate>(instance.Body, parameterList);
        }

        public static Expression<Func<TResult>> ToLambda<TResult>(this Lambda<TResult> instance) {
            return ToLambda<Func<TResult>>(instance);
        }
        public static Expression<Func<T1, TResult>> ToLambda<T1, TResult>(this Lambda<TResult> instance, ParameterExpression arg1) {
            return ToLambda<Func<T1, TResult>>(instance, arg1);
        }
        public static Expression<Func<T1, T2, TResult>> ToLambda<T1, T2, TResult>(this Lambda<TResult> instance, ParameterExpression arg1, ParameterExpression arg2) {
            return ToLambda<Func<T1, T2, TResult>>(instance, arg1, arg2);
        }
        public static Expression<Func<T1, T2, T3, TResult>> ToLambda<T1, T2, T3, TResult>(this Lambda<TResult> instance, ParameterExpression arg1, ParameterExpression arg2, ParameterExpression arg3) {
            return ToLambda<Func<T1, T2, T3, TResult>>(instance, arg1, arg2, arg3);
        }
        public static Expression<Func<T1, T2, T3, T4, TResult>> ToLambda<T1, T2, T3, T4, TResult>(this Lambda<TResult> instance, ParameterExpression arg1, ParameterExpression arg2, ParameterExpression arg3, ParameterExpression arg4) {
            return ToLambda<Func<T1, T2, T3, T4, TResult>>(instance, arg1, arg2, arg3, arg4);
        }
        public static Expression<Func<T1, T2, T3, T4, T5, TResult>> ToLambda<T1, T2, T3, T4, T5, TResult>(this Lambda<TResult> instance, ParameterExpression arg1, ParameterExpression arg2, ParameterExpression arg3, ParameterExpression arg4, ParameterExpression arg5) {
            return ToLambda<Func<T1, T2, T3, T4, T5, TResult>>(instance, arg1, arg2, arg3, arg4, arg5);
        }
        public static Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> ToLambda<T1, T2, T3, T4, T5, T6, TResult>(this Lambda<TResult> instance, ParameterExpression arg1, ParameterExpression arg2, ParameterExpression arg3, ParameterExpression arg4, ParameterExpression arg5, ParameterExpression arg6) {
            return ToLambda<Func<T1, T2, T3, T4, T5, T6, TResult>>(instance, arg1, arg2, arg3, arg4, arg5, arg6);
        }
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, TResult>> ToLambda<T1, T2, T3, T4, T5, T6, T7, TResult>(this Lambda<TResult> instance, ParameterExpression arg1, ParameterExpression arg2, ParameterExpression arg3, ParameterExpression arg4, ParameterExpression arg5, ParameterExpression arg6, ParameterExpression arg7) {
            return ToLambda<Func<T1, T2, T3, T4, T5, T6, T7, TResult>>(instance, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> ToLambda<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Lambda<TResult> instance, ParameterExpression arg1, ParameterExpression arg2, ParameterExpression arg3, ParameterExpression arg4, ParameterExpression arg5, ParameterExpression arg6, ParameterExpression arg7, ParameterExpression arg8) {
            return ToLambda<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>>(instance, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> ToLambda<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Lambda<TResult> instance, ParameterExpression arg1, ParameterExpression arg2, ParameterExpression arg3, ParameterExpression arg4, ParameterExpression arg5, ParameterExpression arg6, ParameterExpression arg7, ParameterExpression arg8, ParameterExpression arg9) {
            return ToLambda<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>>(instance, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> ToLambda<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Lambda<TResult> instance, ParameterExpression arg1, ParameterExpression arg2, ParameterExpression arg3, ParameterExpression arg4, ParameterExpression arg5, ParameterExpression arg6, ParameterExpression arg7, ParameterExpression arg8, ParameterExpression arg9, ParameterExpression arg10) {
            return ToLambda<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>>(instance, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }
#endregion

#region Compile
        public static Func<TResult> Compile<TResult>(this Lambda<TResult> instance) {
            return ToLambda<Func<TResult>>(instance).Compile();
        }
        public static Func<T1, TResult> Compile<T1, TResult>(this Lambda<TResult> instance, ParameterExpression arg1) {
            return ToLambda<Func<T1, TResult>>(instance, arg1).Compile();
        }
        public static Func<T1, T2, TResult> Compile<T1, T2, TResult>(this Lambda<TResult> instance, ParameterExpression arg1, ParameterExpression arg2) {
            return ToLambda<Func<T1, T2, TResult>>(instance, arg1, arg2).Compile();
        }
        public static Func<T1, T2, T3, TResult> Compile<T1, T2, T3, TResult>(this Lambda<TResult> instance, ParameterExpression arg1, ParameterExpression arg2, ParameterExpression arg3) {
            return ToLambda<Func<T1, T2, T3, TResult>>(instance, arg1, arg2, arg3).Compile();
        }
        public static Func<T1, T2, T3, T4, TResult> Compile<T1, T2, T3, T4, TResult>(this Lambda<TResult> instance, ParameterExpression arg1, ParameterExpression arg2, ParameterExpression arg3, ParameterExpression arg4) {
            return ToLambda<Func<T1, T2, T3, T4, TResult>>(instance, arg1, arg2, arg3, arg4).Compile();
        }
        public static Func<T1, T2, T3, T4, T5, TResult> Compile<T1, T2, T3, T4, T5, TResult>(this Lambda<TResult> instance, ParameterExpression arg1, ParameterExpression arg2, ParameterExpression arg3, ParameterExpression arg4, ParameterExpression arg5) {
            return ToLambda<Func<T1, T2, T3, T4, T5, TResult>>(instance, arg1, arg2, arg3, arg4, arg5).Compile();
        }
        public static Func<T1, T2, T3, T4, T5, T6, TResult> Compile<T1, T2, T3, T4, T5, T6, TResult>(this Lambda<TResult> instance, ParameterExpression arg1, ParameterExpression arg2, ParameterExpression arg3, ParameterExpression arg4, ParameterExpression arg5, ParameterExpression arg6) {
            return ToLambda<Func<T1, T2, T3, T4, T5, T6, TResult>>(instance, arg1, arg2, arg3, arg4, arg5, arg6).Compile();
        }
        public static Func<T1, T2, T3, T4, T5, T6, T7, TResult> Compile<T1, T2, T3, T4, T5, T6, T7, TResult>(this Lambda<TResult> instance, ParameterExpression arg1, ParameterExpression arg2, ParameterExpression arg3, ParameterExpression arg4, ParameterExpression arg5, ParameterExpression arg6, ParameterExpression arg7) {
            return ToLambda<Func<T1, T2, T3, T4, T5, T6, T7, TResult>>(instance, arg1, arg2, arg3, arg4, arg5, arg6, arg7).Compile();
        }
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> Compile<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Lambda<TResult> instance, ParameterExpression arg1, ParameterExpression arg2, ParameterExpression arg3, ParameterExpression arg4, ParameterExpression arg5, ParameterExpression arg6, ParameterExpression arg7, ParameterExpression arg8) {
            return ToLambda<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>>(instance, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8).Compile();
        }
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> Compile<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Lambda<TResult> instance, ParameterExpression arg1, ParameterExpression arg2, ParameterExpression arg3, ParameterExpression arg4, ParameterExpression arg5, ParameterExpression arg6, ParameterExpression arg7, ParameterExpression arg8, ParameterExpression arg9) {
            return ToLambda<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>>(instance, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9).Compile();
        }
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> Compile<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Lambda<TResult> instance, ParameterExpression arg1, ParameterExpression arg2, ParameterExpression arg3, ParameterExpression arg4, ParameterExpression arg5, ParameterExpression arg6, ParameterExpression arg7, ParameterExpression arg8, ParameterExpression arg9, ParameterExpression arg10) {
            return ToLambda<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>>(instance, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10).Compile();
        }
#endregion

#region Evaluate
        public static object Evaluate(this Expression instance) {
            return Expression.Lambda<Func<object>>(instance, Array.Empty<ParameterExpression>()).Compile().Invoke();
        }
        public static TResult Evaluate<TResult>(this Expression instance) {
            return Expression.Lambda<Func<TResult>>(instance, Array.Empty<ParameterExpression>()).Compile().Invoke();
        }
        public static object Evaluate(this LambdaExpression instance, params object[] parameters) {
            return instance.Compile().DynamicInvoke(parameters);
        }
        public static TResult Evaluate<TResult>(this LambdaExpression instance, params object[] parameters) {
            return (TResult)Evaluate(instance, parameters);
        }
        public static TResult Evaluate<TResult>(this Expression<Func<TResult>> instance) {
            return instance.Compile().Invoke();
        }
        public static TResult Evaluate<T1, TResult>(this Expression<Func<T1, TResult>> instance, T1 arg1) {
            return instance.Compile().Invoke(arg1);
        }
        public static TResult Evaluate<T1, T2, TResult>(this Expression<Func<T1, T2, TResult>> instance, T1 arg1, T2 arg2) {
            return instance.Compile().Invoke(arg1, arg2);
        }
        public static TResult Evaluate<T1, T2, T3, TResult>(this Expression<Func<T1, T2, T3, TResult>> instance, T1 arg1, T2 arg2, T3 arg3) {
            return instance.Compile().Invoke(arg1, arg2, arg3);
        }
        public static TResult Evaluate<T1, T2, T3, T4, TResult>(this Expression<Func<T1, T2, T3, T4, TResult>> instance, T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
            return instance.Compile().Invoke(arg1, arg2, arg3, arg4);
        }
        public static TResult Evaluate<T1, T2, T3, T4, T5, TResult>(this Expression<Func<T1, T2, T3, T4, T5, TResult>> instance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
            return instance.Compile().Invoke(arg1, arg2, arg3, arg4, arg5);
        }
        public static TResult Evaluate<T1, T2, T3, T4, T5, T6, TResult>(this Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> instance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) {
            return instance.Compile().Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
        }
        public static TResult Evaluate<T1, T2, T3, T4, T5, T6, T7, TResult>(this Expression<Func<T1, T2, T3, T4, T5, T6, T7, TResult>> instance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) {
            return instance.Compile().Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
        public static TResult Evaluate<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> instance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) {
            return instance.Compile().Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
        public static TResult Evaluate<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> instance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) {
            return instance.Compile().Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }
        public static TResult Evaluate<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> instance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) {
            return instance.Compile().Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }
#endregion
    }
}