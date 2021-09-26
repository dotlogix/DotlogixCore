// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  RewritingExpressionVisitor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 08.05.2021 23:35
// LastEdited:  26.09.2021 22:15
// ==================================================

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DotLogix.Core.Expressions {
    public class RewritingExpressionVisitor : ExpressionVisitor
    {
        protected Dictionary<MethodInfo, IMethodCallRewriter> MethodMappings { get; } = new Dictionary<MethodInfo, IMethodCallRewriter>();
        protected Dictionary<MemberInfo, IMemberRewriter> MemberMappings { get; } = new Dictionary<MemberInfo, IMemberRewriter>();
        protected Dictionary<ParameterExpression, IParameterRewriter> ParameterMappings { get; } = new Dictionary<ParameterExpression, IParameterRewriter>();
        protected List<IExpressionRewriter> DynamicMappings { get; } = new List<IExpressionRewriter>();

        public void Use(IExpressionRewriter rewriter)
        {
            switch (rewriter)
            {
                case IMemberRewriter memberRewriter:
                    Use(memberRewriter);
                    break;
                case IMethodCallRewriter methodCallRewriter:
                    Use(methodCallRewriter);
                    break;
                case IParameterRewriter parameterRewriter:
                    Use(parameterRewriter);
                    break;
                default:
                    DynamicMappings.Add(rewriter);
                    break;
            }
        }
        
        public void Use(IParameterRewriter parameterRewriter)
        {
            ParameterMappings[parameterRewriter.Parameter] = parameterRewriter;
        }
        
        public void Use(IMethodCallRewriter methodCallRewriter)
        {
            MethodMappings[methodCallRewriter.MatchesMethod] = methodCallRewriter;
        }
        
        public void Use(IMemberRewriter memberRewriter)
        {
            MemberMappings[memberRewriter.MemberInfo] = memberRewriter;
        }

        /// <inheritdoc />
        public override Expression Visit(Expression node)
        {
            if (DynamicMappings.Count != 0)
            {
                var rewriter = DynamicMappings.FirstOrDefault(r => r.CanRewrite(node));
                node = rewriter?.Rewrite(node) ?? node;
            }

            return base.Visit(node);
        }


        /// <inheritdoc />
        protected override Expression VisitMember(MemberExpression node)
        {
            if (MemberMappings.TryGetValue(node.Member, out var rewriter)) {
                return Visit(rewriter.Rewrite(node)!)!;
            }
            
            return base.VisitMember(node);
        }
        
        /// <inheritdoc />
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (MethodMappings.TryGetValue(node.Method, out var rewriter)) {
                return Visit(rewriter.Rewrite(node)!)!;
            }
            
            return base.VisitMethodCall(node);
        }

        /// <inheritdoc />
        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (ParameterMappings.TryGetValue(node, out var rewriter)) {
                return Visit(rewriter.Rewrite(node)!)!;
            }
            return base.VisitParameter(node);
        }
    }
}