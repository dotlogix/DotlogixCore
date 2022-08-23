// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  RewritingExpressionVisitor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 08.05.2021 23:35
// LastEdited:  26.09.2021 22:15
// ==================================================

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DotLogix.Core.Expressions.Rewriters; 

/// <summary>
///     A class to rewrite expressions
/// </summary>
public class RewritingExpressionVisitor : ExpressionVisitor {
    private readonly List<IParameterRewriter> _parameterRewriters = new();
    private readonly List<IMemberRewriter> _memberRewriters = new();
    private readonly List<IMethodCallRewriter> _methodRewriters = new();
    private readonly List<IExpressionRewriter> _rewriters = new();

    /// <summary>
    ///     The registered rewriters for <see cref="MethodCallExpression" />
    /// </summary>
    public IReadOnlyCollection<IMethodCallRewriter> MethodRewriters => _methodRewriters;

    /// <summary>
    ///     The registered rewriters for <see cref="MemberExpression" />
    /// </summary>
    public IReadOnlyCollection<IMemberRewriter> MemberRewriters => _memberRewriters;

    /// <summary>
    ///     The registered rewriters for <see cref="Expression" />
    /// </summary>
    public IReadOnlyCollection<IExpressionRewriter> Rewriters => _rewriters;
        
    /// <summary>
    ///     Adds a rewriter for <see cref="Expression" />
    /// </summary>
    public void Add(IExpressionRewriter rewriter) {
        _rewriters.Add(rewriter);
    }
        
    /// <summary>
    ///     Adds a rewriters for <see cref="Expression" />
    /// </summary>
    public void AddRange(IEnumerable<IExpressionRewriter> rewriters) {
        _rewriters.AddRange(rewriters);
    }
        
    /// <summary>
    ///     Adds a rewriter for <see cref="ParameterExpression" />
    /// </summary>
    public void Add(IParameterRewriter rewriter) {
        _parameterRewriters.Add(rewriter);
    }
        
    /// <summary>
    ///     Adds a rewriters for <see cref="ParameterExpression" />
    /// </summary>
    public void AddRange(IEnumerable<IParameterRewriter> rewriters) {
        _parameterRewriters.AddRange(rewriters);
    }
        
    /// <summary>
    ///     Adds a rewriters for <see cref="MethodCallExpression" />
    /// </summary>
    public void AddRange(IEnumerable<IMethodCallRewriter> methodCallRewriters) {
        _methodRewriters.AddRange(methodCallRewriters);
    }

    /// <summary>
    ///     Adds a rewriter for <see cref="MemberExpression" />
    /// </summary>
    public void Add(IMemberRewriter memberRewriter) {
        _memberRewriters.Add(memberRewriter);
    }

    /// <summary>
    ///     Adds a rewriter for <see cref="MethodCallExpression" />
    /// </summary>
    public void Add(IMethodCallRewriter methodCallRewriter) {
        _methodRewriters.Add(methodCallRewriter);
    }
        
    /// <summary>
    ///     Adds a rewriters for <see cref="MemberExpression" />
    /// </summary>
    public void AddRange(IEnumerable<IMemberRewriter> memberRewriters) {
        _memberRewriters.AddRange(memberRewriters);
    }

    /// <summary>
    ///     Removes a rewriter for <see cref="Expression" />
    /// </summary>
    public bool Remove(IExpressionRewriter rewriter) {
        return _rewriters.Remove(rewriter);
    }

    /// <summary>
    ///     Removes a rewriter for <see cref="MethodCallExpression" />
    /// </summary>
    public bool Remove(IMethodCallRewriter methodCallRewriter) {
        return _methodRewriters.Remove(methodCallRewriter);
    }

    /// <summary>
    ///     Removes a rewriter for <see cref="MemberExpression" />
    /// </summary>
    public bool Remove(IMemberRewriter memberRewriter) {
        return _memberRewriters.Remove(memberRewriter);
    }

    /// <inheritdoc />
    public override Expression Visit(Expression node) {
        if(RewriteOrDefault(_rewriters, r => r.Rewrite(node)) is { } rewritten)
            return base.Visit(rewritten);
        return base.Visit(node);
    }

    /// <inheritdoc />
    protected override Expression VisitMember(MemberExpression node) {
        var rewritten = RewriteOrDefault(_memberRewriters, r => r.Rewrite(node.Expression, node.Member));
        return rewritten is null
            ? base.VisitMember(node)
            : base.Visit(rewritten)!;
    }

    /// <inheritdoc />
    protected override Expression VisitMethodCall(MethodCallExpression node) {
        var rewritten = RewriteOrDefault(_methodRewriters, r => r.Rewrite(node.Object, node.Method, node.Arguments));
        return rewritten is null
            ? base.VisitMethodCall(node)
            : base.Visit(rewritten)!;
    }

    protected override Expression VisitParameter(ParameterExpression node) {
        var rewritten = RewriteOrDefault(_parameterRewriters, r => r.Rewrite(node));
        return rewritten is null
            ? base.VisitParameter(node)
            : base.Visit(rewritten)!;
    }

    private static Expression RewriteOrDefault<T>(IReadOnlyCollection<T> rewriters, Func<T, Expression> rewrite) {
        if(rewriters.Count == 0)
            return default;
            
        foreach(var rewriter in rewriters) {
            if(rewrite.Invoke(rewriter) is { } rewritten)
                return rewritten;
        }
        return default;
    }
}