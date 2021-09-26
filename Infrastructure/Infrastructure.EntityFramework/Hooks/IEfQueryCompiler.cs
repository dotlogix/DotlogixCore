using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace DotLogix.Infrastructure.EntityFramework.Hooks {
    [SuppressMessage("ReSharper", "EF1001")]
    public interface IEfQueryCompiler : IQueryCompiler{
        event EventHandler<QueryResultEventArgs> QueryExecuting;
        event EventHandler<QueryResultEventArgs> QueryExecuted;
        event EventHandler<QueryResultEventArgs> QueryFailed;
        event EventHandler<QueryCompileEventArgs> QueryCompile;
    }
}