using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DotLogix.Core.Expressions;
using DotLogix.Core.Expressions.Rewriters;

namespace DotLogix.Core.Expressions {
    public static partial class Lambdas {
        public static Lambda<TResult> Invoke<TResult>(LambdaExpression expression, params Lambda[] args) {
            return Invoke(expression, args.AsEnumerable()).Body;
        }

        public static Lambda<TResult> Invoke<TResult>(LambdaExpression expression, IEnumerable<Lambda> args) {
            return Invoke(expression, args).Body;
        }

        public static Lambda Invoke(LambdaExpression expression, params Lambda[] args) {
            return Invoke(expression, args.AsEnumerable());
        }

        public static Lambda Invoke(LambdaExpression expression, IEnumerable<Lambda> args) {
            var rewriter = new RewritingExpressionVisitor();
            var parameters = expression.Parameters;
            var idx = 0;
            foreach(var arg in args) {
                var parameter = parameters[idx];
                var argument = arg.Cast(parameter.Type);
                rewriter.RewriteParameter(parameter, argument.Body);
                idx++;
            }

            if(idx < parameters.Count) {
                throw new ArgumentException($"Not enough parameters. Expected {parameters.Count}, but got {idx}");
            }

            return rewriter.Visit(expression.Body);
        }

        public static Lambda<TResult> Invoke<TResult>(Expression<Func<TResult>> expression) {
            return Invoke<TResult>((LambdaExpression)expression);
        }

        public static Lambda<TResult> Invoke<T1, TResult>(Expression<Func<T1, TResult>> expression, Lambda<T1> arg1) {
            return Invoke<TResult>((LambdaExpression)expression, arg1);
        }

        public static Lambda<TResult> Invoke<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> expression, Lambda<T1> arg1, Lambda<T2> arg2) {
            return Invoke<TResult>((LambdaExpression)expression, arg1, arg2);
        }

        public static Lambda<TResult> Invoke<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> expression, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3) {
            return Invoke<TResult>((LambdaExpression)expression, arg1, arg2, arg3);
        }

        public static Lambda<TResult> Invoke<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> expression, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4) {
            return Invoke<TResult>((LambdaExpression)expression, arg1, arg2, arg3, arg4);
        }

        public static Lambda<TResult> Invoke<T1, T2, T3, T4, T5, TResult>(Expression<Func<T1, T2, T3, T4, T5, TResult>> expression, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5) {
            return Invoke<TResult>((LambdaExpression)expression, arg1, arg2, arg3, arg4, arg5);
        }

        public static Lambda<TResult> Invoke<T1, T2, T3, T4, T5, T6, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> expression, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5, Lambda<T6> arg6) {
            return Invoke<TResult>((LambdaExpression)expression, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        public static Lambda<TResult> Invoke<T1, T2, T3, T4, T5, T6, T7, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, TResult>> expression, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5, Lambda<T6> arg6, Lambda<T7> arg7) {
            return Invoke<TResult>((LambdaExpression)expression, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        public static Lambda<TResult> Invoke<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> expression, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5, Lambda<T6> arg6, Lambda<T7> arg7, Lambda<T8> arg8) {
            return Invoke<TResult>((LambdaExpression)expression, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        public static Lambda<TResult> Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> expression, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5, Lambda<T6> arg6, Lambda<T7> arg7, Lambda<T8> arg8, Lambda<T9> arg9) {
            return Invoke<TResult>((LambdaExpression)expression, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        public static Lambda<TResult> Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> expression, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5, Lambda<T6> arg6, Lambda<T7> arg7, Lambda<T8> arg8, Lambda<T9> arg9, Lambda<T10> arg10) {
            return Invoke<TResult>((LambdaExpression)expression, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        public static Lambda<TResult> Invoke<T1, TResult>(this Lambda<T1> arg1, Expression<Func<T1, TResult>> expression) {
            return Invoke<TResult>((LambdaExpression)expression, arg1);
        }

        public static Lambda<TResult> Invoke<T1, T2, TResult>(this Lambda<T1> arg1, Expression<Func<T1, T2, TResult>> expression, Lambda<T2> arg2) {
            return Invoke<TResult>((LambdaExpression)expression, arg1, arg2);
        }

        public static Lambda<TResult> Invoke<T1, T2, T3, TResult>(this Lambda<T1> arg1, Expression<Func<T1, T2, T3, TResult>> expression, Lambda<T2> arg2, Lambda<T3> arg3) {
            return Invoke<TResult>((LambdaExpression)expression, arg1, arg2, arg3);
        }

        public static Lambda<TResult> Invoke<T1, T2, T3, T4, TResult>(this Lambda<T1> arg1, Expression<Func<T1, T2, T3, T4, TResult>> expression, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4) {
            return Invoke<TResult>((LambdaExpression)expression, arg1, arg2, arg3, arg4);
        }

        public static Lambda<TResult> Invoke<T1, T2, T3, T4, T5, TResult>(this Lambda<T1> arg1, Expression<Func<T1, T2, T3, T4, T5, TResult>> expression, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5) {
            return Invoke<TResult>((LambdaExpression)expression, arg1, arg2, arg3, arg4, arg5);
        }

        public static Lambda<TResult> Invoke<T1, T2, T3, T4, T5, T6, TResult>(this Lambda<T1> arg1, Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> expression, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5, Lambda<T6> arg6) {
            return Invoke<TResult>((LambdaExpression)expression, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        public static Lambda<TResult> Invoke<T1, T2, T3, T4, T5, T6, T7, TResult>(this Lambda<T1> arg1, Expression<Func<T1, T2, T3, T4, T5, T6, T7, TResult>> expression, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5, Lambda<T6> arg6, Lambda<T7> arg7) {
            return Invoke<TResult>((LambdaExpression)expression, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        public static Lambda<TResult> Invoke<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Lambda<T1> arg1, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> expression, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5, Lambda<T6> arg6, Lambda<T7> arg7, Lambda<T8> arg8) {
            return Invoke<TResult>((LambdaExpression)expression, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        public static Lambda<TResult> Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Lambda<T1> arg1, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> expression, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5, Lambda<T6> arg6, Lambda<T7> arg7, Lambda<T8> arg8, Lambda<T9> arg9) {
            return Invoke<TResult>((LambdaExpression)expression, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        public static Lambda<TResult> Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Lambda<T1> arg1, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> expression, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5, Lambda<T6> arg6, Lambda<T7> arg7, Lambda<T8> arg8, Lambda<T9> arg9, Lambda<T10> arg10) {
            return Invoke<TResult>((LambdaExpression)expression, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

    }
}