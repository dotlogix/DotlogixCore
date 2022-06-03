using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Core.Expressions;
using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.WebServices.EntityFramework.Context.Events;
using DotLogix.WebServices.EntityFramework.Database.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace DotLogix.WebServices.EntityFramework.Database {
    
    [SuppressMessage("ReSharper", "EF1001")]
    public class EntityQueryCompiler : QueryCompiler, IEntityQueryCompiler {
        public event EventHandler<QueryResultEventArgs> QueryExecuting;
        public event EventHandler<QueryResultEventArgs> QueryExecuted;
        public event EventHandler<QueryResultEventArgs> QueryFailed;
        public event EventHandler<QueryCompileEventArgs> QueryCompile;
        
        private static MethodInfo ExecuteQueryAsyncMethod { get; } = typeof(EntityQueryCompiler).GetMethod(nameof(OnExecuteQueryAsync), BindingFlags.Instance | BindingFlags.NonPublic);
        private static MethodInfo ExecuteQueryEnumerableMethod { get; } = typeof(EntityQueryCompiler).GetMethod(nameof(OnExecuteQueryEnumerableAsync), BindingFlags.Instance | BindingFlags.NonPublic);
        private static MethodInfo ExecuteQueryMethod { get; } = typeof(EntityQueryCompiler).GetMethod(nameof(OnExecuteQuery), BindingFlags.Instance | BindingFlags.NonPublic);

        public EntityQueryCompiler(
            IQueryContextFactory queryContextFactory,
            ICompiledQueryCache compiledQueryCache, 
            ICompiledQueryCacheKeyGenerator compiledQueryCacheKeyGenerator,
            IDatabase database,
            IDiagnosticsLogger<DbLoggerCategory.Query> logger,
            ICurrentDbContext currentContext,
            IEvaluatableExpressionFilter expressionFilter,
            IModel model
        ) : base(queryContextFactory, compiledQueryCache, compiledQueryCacheKeyGenerator, database, logger, currentContext, expressionFilter, model) {
        }

        public override Func<QueryContext, TResult> CompileQueryCore<TResult>(IDatabase database, Expression query, IModel model, bool async) {
            var compileEventArgs = new QueryCompileEventArgs(query);
            OnCompileQuery(compileEventArgs);
            query = compileEventArgs.Expression;
            
            var queryFunc = base.CompileQueryCore<TResult>(database, query, model, async);

            var resultType = typeof(TResult);
            var method = ExecuteQueryMethod;
            if (async && resultType.IsGenericType)
            {
                var typeDefinition = resultType.GetGenericTypeDefinition();
                var genericArguments = resultType.GetGenericArguments();
                if (typeDefinition == typeof(Task<>))
                {
                    resultType = genericArguments[0];
                    method = ExecuteQueryAsyncMethod;
                }
                else if (typeDefinition == typeof(IAsyncEnumerable<>))
                {
                    resultType = genericArguments[0];
                    method = ExecuteQueryEnumerableMethod;
                }
            }
            
            var genericMethod = method.MakeGenericMethod(resultType).CreateDynamicInvoke();
            return ctx => (TResult)genericMethod.Invoke(this, ctx, query, queryFunc);
        }
        
        private async Task<TResult> OnExecuteQueryAsync<TResult>(QueryContext ctx, Expression query, Func<QueryContext, Task<TResult>> executeFunc) {
            var resultEventArgs = new QueryResultEventArgs(ctx, query, typeof(TResult));
            OnQueryExecuting(resultEventArgs);

            if(resultEventArgs.HasResult == false) {
                try {
                    var result = await executeFunc.Invoke(ctx);
                    resultEventArgs.SetResult(result);
                } catch(Exception e) {
                    resultEventArgs.SetException(e);
                    OnQueryFailed(resultEventArgs);

                    if(resultEventArgs.Exception != null) {
                        throw;
                    }
                }
            }
            
            OnQueryExecuted(resultEventArgs);
            return (TResult)resultEventArgs.Result;
        }
        
        private IAsyncEnumerable<TResult> OnExecuteQueryEnumerableAsync<TResult>(QueryContext ctx, Expression query, Func<QueryContext, IAsyncEnumerable<TResult>> executeFunc)
        {
            async Task<IReadOnlyList<TResult>> GetResultsAsync(CancellationToken cancellationToken)
            {
                var resultEventArgs = new QueryResultEventArgs(ctx, query, typeof(IReadOnlyCollection<TResult>));
                OnQueryExecuting(resultEventArgs);

                if (resultEventArgs.HasResult == false)
                {
                    IAsyncEnumerator<TResult> resultEnumerator = null;
                    try
                    {
                        resultEnumerator = executeFunc.Invoke(ctx).GetAsyncEnumerator(cancellationToken);
                        var results = new List<TResult>();
                        while (await resultEnumerator.MoveNextAsync())
                        {
                            results.Add(resultEnumerator.Current);
                        }
                        resultEventArgs.SetResult(results);
                    }
                    catch (Exception e)
                    {
                        resultEventArgs.SetException(e);
                        OnQueryFailed(resultEventArgs);

                        if (resultEventArgs.Exception != null)
                        {
                            throw;
                        }
                    }
                    finally
                    {
                        if(resultEnumerator is not null)
                        {
                            await resultEnumerator.DisposeAsync();
                        }
                    }
                }

                OnQueryExecuted(resultEventArgs);
                return ((IReadOnlyCollection<TResult>)resultEventArgs.Result).AsReadOnlyList();
            }
            return new BufferedAsyncEnumerable<TResult>(GetResultsAsync);
        }
        
        private TResult OnExecuteQuery<TResult>(QueryContext ctx, Expression query, Func<QueryContext, TResult> executeFunc) {
            var resultEventArgs = new QueryResultEventArgs(ctx, query, typeof(TResult));
            OnQueryExecuting(resultEventArgs);

            if(resultEventArgs.HasResult == false) {
                try {
                    var result = executeFunc.Invoke(ctx);
                    resultEventArgs.SetResult(result);
                } catch(Exception e) {
                    resultEventArgs.SetException(e);
                    OnQueryFailed(resultEventArgs);

                    if(resultEventArgs.Exception != null) {
                        throw;
                    }
                }
            }
            
            OnQueryExecuted(resultEventArgs);
            return (TResult)resultEventArgs.Result;
        }

        protected virtual void OnQueryExecuting(QueryResultEventArgs eventArgs) {
            QueryExecuting?.Invoke(this, eventArgs);
        }
        protected virtual void OnQueryExecuted(QueryResultEventArgs eventArgs) {
            QueryExecuted?.Invoke(this, eventArgs);
        }
        protected virtual void OnQueryFailed(QueryResultEventArgs eventArgs) {
            QueryFailed?.Invoke(this, eventArgs);
        }
        protected virtual void OnCompileQuery(QueryCompileEventArgs eventArgs) {
            QueryCompile?.Invoke(this, eventArgs);
        }
    }
}