// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  ExpressionRewriter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 08.05.2021 23:34
// LastEdited:  26.09.2021 22:15
// ==================================================

using System;
using System.Linq.Expressions;

namespace DotLogix.Core.Expressions {
    public class ExpressionRewriter : IExpressionRewriter
    {
        public Expression Rewrite(Expression expression)
        {
            return Callback.Invoke(expression);
        }

        public Func<Expression, Expression> Callback { get; set; }
        public Func<Expression, bool> Condition { get; set; }

        public bool CanRewrite(Expression expression)
        {
            return Condition?.Invoke(expression) ?? true;
        }
    }
}
