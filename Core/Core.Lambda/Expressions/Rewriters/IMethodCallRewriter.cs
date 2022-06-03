// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  IMethodCallRewriter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 08.05.2021 23:35
// LastEdited:  26.09.2021 22:15
// ==================================================

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace DotLogix.Core.Expressions.Rewriters {
    public interface IMethodCallRewriter
    {
        Expression Rewrite(Expression instance, MethodInfo method, IReadOnlyList<Expression> arguments);
    }

    public class MethodCallRewriter : IMethodCallRewriter {
        private readonly Func<Expression, MethodInfo, IReadOnlyList<Expression>, Expression> _rewrite;

        public MethodCallRewriter(Func<Expression, MethodInfo, IReadOnlyList<Expression>, Expression> rewrite) {
            _rewrite = rewrite;
        }

        public Expression Rewrite(Expression instance, MethodInfo method, IReadOnlyList<Expression> arguments) {
            return _rewrite.Invoke(instance, method, arguments);
        }
    }
}