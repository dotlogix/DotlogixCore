// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  IExpressionRewriter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 08.05.2021 23:35
// LastEdited:  26.09.2021 22:15
// ==================================================

using System;
using System.Linq.Expressions;

namespace DotLogix.Core.Expressions.Rewriters; 

public interface IExpressionRewriter
{
    Expression Rewrite(Expression expression);
}

public class ExpressionRewriter : IExpressionRewriter {
    private readonly Func<Expression, Expression> _rewrite;

    public ExpressionRewriter(Func<Expression, Expression> rewrite) {
        _rewrite = rewrite;
    }

    public Expression Rewrite(Expression expression) {
        return _rewrite.Invoke(expression);
    }
}