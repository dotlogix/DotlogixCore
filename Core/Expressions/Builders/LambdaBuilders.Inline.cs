// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  LambdaBuilders.Inline.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 09.06.2021 00:05
// LastEdited:  26.09.2021 22:15
// ==================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DotLogix.Core.Expressions {
    public static partial class LambdaBuilders {
        public static LambdaBuilder<TResult> Inline<TResult>(LambdaExpression expression, params LambdaBuilder[] args) {
            return Inline(expression, args.AsEnumerable()).Body;
        }

        public static LambdaBuilder<TResult> Inline<TResult>(LambdaExpression expression, IEnumerable<LambdaBuilder> args) {
            return Inline(expression, args).Body;
        }

        public static LambdaBuilder Inline(LambdaExpression expression, params LambdaBuilder[] args) {
            return Inline(expression, args.AsEnumerable());
        }

        public static LambdaBuilder Inline(LambdaExpression expression, IEnumerable<LambdaBuilder> args) {
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

            var rewritten = rewriter.Visit(expression.Body);
            return new LambdaBuilder(rewritten, expression.Type);
        }

        public static LambdaBuilder<TResult> Inline<T, TResult>(Expression<Func<T, TResult>> expression) {
            return Inline<TResult>((LambdaExpression)expression);
        }
        public static LambdaBuilder<TResult> Inline<T, T1, TResult>(Expression<Func<T, T1, TResult>> expression, LambdaBuilder<T1> arg1) {
            return Inline<TResult>((LambdaExpression)expression, arg1);
        }
        public static LambdaBuilder<TResult> Inline<T, T1, T2, TResult>(Expression<Func<T, T1, T2, TResult>> expression, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2) {
            return Inline<TResult>((LambdaExpression)expression, arg1, arg2);
        }
        public static LambdaBuilder<TResult> Inline<T, T1, T2, T3, TResult>(Expression<Func<T, T1, T2, T3, TResult>> expression, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3) {
            return Inline<TResult>((LambdaExpression)expression, arg1, arg2, arg3);
        }
        public static LambdaBuilder<TResult> Inline<T, T1, T2, T3, T4, TResult>(Expression<Func<T, T1, T2, T3, T4, TResult>> expression, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3, LambdaBuilder<T4> arg4) {
            return Inline<TResult>((LambdaExpression)expression, arg1, arg2, arg3, arg4);
        }
        public static LambdaBuilder<TResult> Inline<T, T1, T2, T3, T4, T5, TResult>(Expression<Func<T, T1, T2, T3, T4, T5, TResult>> expression, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3, LambdaBuilder<T4> arg4, LambdaBuilder<T5> arg5) {
            return Inline<TResult>((LambdaExpression)expression, arg1, arg2, arg3, arg4, arg5);
        }
        public static LambdaBuilder<TResult> Inline<T, T1, T2, T3, T4, T5, T6, TResult>(Expression<Func<T, T1, T2, T3, T4, T5, T6, TResult>> expression, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3, LambdaBuilder<T4> arg4, LambdaBuilder<T5> arg5, LambdaBuilder<T6> arg6) {
            return Inline<TResult>((LambdaExpression)expression, arg1, arg2, arg3, arg4, arg5, arg6);
        }
        public static LambdaBuilder<TResult> Inline<T, T1, T2, T3, T4, T5, T6, T7, TResult>(Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, TResult>> expression, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3, LambdaBuilder<T4> arg4, LambdaBuilder<T5> arg5, LambdaBuilder<T6> arg6, LambdaBuilder<T7> arg7) {
            return Inline<TResult>((LambdaExpression)expression, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
        public static LambdaBuilder<TResult> Inline<T, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, TResult>> expression, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3, LambdaBuilder<T4> arg4, LambdaBuilder<T5> arg5, LambdaBuilder<T6> arg6, LambdaBuilder<T7> arg7, LambdaBuilder<T8> arg8) {
            return Inline<TResult>((LambdaExpression)expression, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
        public static LambdaBuilder<TResult> Inline<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> expression, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3, LambdaBuilder<T4> arg4, LambdaBuilder<T5> arg5, LambdaBuilder<T6> arg6, LambdaBuilder<T7> arg7, LambdaBuilder<T8> arg8, LambdaBuilder<T9> arg9) {
            return Inline<TResult>((LambdaExpression)expression, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }
        public static LambdaBuilder<TResult> Inline<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> expression, LambdaBuilder<T1> arg1, LambdaBuilder<T2> arg2, LambdaBuilder<T3> arg3, LambdaBuilder<T4> arg4, LambdaBuilder<T5> arg5, LambdaBuilder<T6> arg6, LambdaBuilder<T7> arg7, LambdaBuilder<T8> arg8, LambdaBuilder<T9> arg9, LambdaBuilder<T10> arg10) {
            return Inline<TResult>((LambdaExpression)expression, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }
    }
}