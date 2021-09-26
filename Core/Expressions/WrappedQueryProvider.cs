// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  WrappedQueryProvider.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 10.05.2021 22:49
// LastEdited:  26.09.2021 22:15
// ==================================================

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Core.Expressions {
    public class WrappedQueryProvider : IQueryProvider {
        protected static readonly MethodInfo CreateQueryMethodInfo = typeof(WrappedQueryProvider).GetRuntimeMethods().Single(m => (m.Name == nameof(CreateQuery)) && m.IsGenericMethod);
        protected static readonly MethodInfo ExecuteQueryMethodInfo = typeof(WrappedQueryProvider).GetRuntimeMethods().Single(m => (m.Name == nameof(Execute)) && m.IsGenericMethod);
        protected static readonly ConcurrentDictionary<Type, DynamicInvoke> CreateQueryCache = new ConcurrentDictionary<Type, DynamicInvoke>();
        protected static readonly ConcurrentDictionary<Type, DynamicInvoke> ExecuteQueryCache = new ConcurrentDictionary<Type, DynamicInvoke>();

        protected IQueryProvider InnerProvider { get; }

        public WrappedQueryProvider(IQueryProvider innerProvider) {
            InnerProvider = innerProvider;
        }

        public IQueryable CreateQuery(Expression expression) {
            var method =  CreateQueryCache.GetOrAdd(expression.Type, MakeGenericCreateMethod);
            if(method == null) {
                throw new NotSupportedException($"Can not create query for type {expression.Type.GetFriendlyGenericName()}");
            }
            return (IQueryable)method.Invoke(this, expression);
        }
        public virtual IQueryable<TElement> CreateQuery<TElement>(Expression expression) {
            var newQueryable = InnerProvider.CreateQuery<TElement>(expression);
            return new WrappedQueryable<TElement>(this, newQueryable);
        }

        public object Execute(Expression expression) {
            var method =  ExecuteQueryCache.GetOrAdd(expression.Type, MakeGenericExecuteMethod);
            if(method == null) {
                throw new NotSupportedException($"Can not execute query for type {expression.Type.GetFriendlyGenericName()}");
            }
            return method.Invoke(this, expression);
        }
        
        public virtual TResult Execute<TResult>(Expression expression) {
            var queryExpression = expression;
            try {
                queryExpression = OnInterceptQuery(queryExpression);
                var result = InnerProvider.Execute<TResult>(queryExpression);
                return (TResult)OnInterceptQueryResult(queryExpression, result);
            } catch(Exception e) {
                if(OnInterceptQueryError(queryExpression, e, out var obj)) {
                    return (TResult)OnInterceptQueryResult(queryExpression, obj);
                }
                throw;
            }
        }
        
        public virtual Expression OnInterceptQuery(Expression expression) {
            return expression;
        }

        protected virtual object OnInterceptQueryResult(Expression expression, object result) {
            return result;
        }
        
        protected virtual bool OnInterceptQueryError(Expression queryExpression, Exception exception, out object result) {
            result = default;
            return false;
        }
        
        protected virtual DynamicInvoke MakeGenericCreateMethod(Type elementType) {
            return CreateQueryMethodInfo.MakeGenericMethod(elementType).CreateDynamicInvoke();
        }
        
        protected virtual DynamicInvoke MakeGenericExecuteMethod(Type resultType) {
            return ExecuteQueryMethodInfo.MakeGenericMethod(resultType).CreateDynamicInvoke();
        }
    }
}