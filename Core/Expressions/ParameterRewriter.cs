// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  ParameterRewriter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 08.05.2021 23:35
// LastEdited:  26.09.2021 22:15
// ==================================================

using System;
using System.Linq.Expressions;

namespace DotLogix.Core.Expressions {
    public class ParameterRewriter : IParameterRewriter
    {
        public Func<ParameterExpression, Expression> Callback { get; set; }
        public ParameterExpression Parameter { get; set; }

        public Expression Rewrite(Expression expression) => Rewrite((ParameterExpression) expression);
        public Expression Rewrite(ParameterExpression expression)
        {
            return Callback.Invoke(expression);
        }

        public bool CanRewrite(Expression expression)
        {
            return expression == Parameter;
        }
    }
}