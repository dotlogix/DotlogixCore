using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Dynamics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace DotLogix.Infrastructure.EntityFramework.Hooks {
    [SuppressMessage("ReSharper", "EF1001")]
    public class EfEventQueryCompiler : QueryCompiler, IEfQueryCompiler {
        public event EventHandler<QueryResultEventArgs> QueryExecuting;
        public event EventHandler<QueryResultEventArgs> QueryExecuted;
        public event EventHandler<QueryResultEventArgs> QueryFailed;
        public event EventHandler<QueryCompileEventArgs> QueryCompile;
        
        private static MethodInfo ExecuteQueryAsyncMethod { get; } = typeof(EfEventQueryCompiler).GetMethod(nameof(OnExecuteQueryAsync), BindingFlags.Instance | BindingFlags.NonPublic);
        private static MethodInfo ExecuteQueryMethod { get; } = typeof(EfEventQueryCompiler).GetMethod(nameof(OnExecuteQuery), BindingFlags.Instance | BindingFlags.NonPublic);
        
        public EfEventQueryCompiler(
            IQueryContextFactory queryContextFactory, ICompiledQueryCache compiledQueryCache, ICompiledQueryCacheKeyGenerator compiledQueryCacheKeyGenerator,
            IDatabase database, IDiagnosticsLogger<DbLoggerCategory.Query> logger, ICurrentDbContext currentContext, IEvaluatableExpressionFilter expressionFilter, IModel model)
            : base(queryContextFactory, compiledQueryCache, compiledQueryCacheKeyGenerator, database, logger, currentContext, expressionFilter, model) {
            
        }

        public override Func<QueryContext, TResult> CompileQueryCore<TResult>(IDatabase database, Expression query, IModel model, bool async) {
            var compileEventArgs = new QueryCompileEventArgs(query);
            OnCompileQuery(compileEventArgs);

            query = compileEventArgs.Expression;
            var queryFunc = base.CompileQueryCore<TResult>(database, query, model, async);

            var resultType = typeof(TResult);
            MethodInfo method;
            if(async && resultType.IsAssignableToGeneric(typeof(Task<>), out var typeArguments)) {
                resultType = typeArguments[0];
                method = ExecuteQueryAsyncMethod;
            } else {
                method = ExecuteQueryMethod;
            }

            var compiledFunc = method.MakeGenericMethod(resultType).CreateDynamicInvoke();
            return ctx => (TResult)compiledFunc.Invoke(this, ctx, query, queryFunc);
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