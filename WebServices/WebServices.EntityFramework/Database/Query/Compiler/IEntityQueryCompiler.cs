using System;
using System.Diagnostics.CodeAnalysis;
using DotLogix.WebServices.EntityFramework.Context.Events;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace DotLogix.WebServices.EntityFramework.Database; 

[SuppressMessage("ReSharper", "EF1001")]
public interface IEntityQueryCompiler : IQueryCompiler{
    event EventHandler<QueryResultEventArgs> QueryExecuting;
    event EventHandler<QueryResultEventArgs> QueryExecuted;
    event EventHandler<QueryResultEventArgs> QueryFailed;
    event EventHandler<QueryCompileEventArgs> QueryCompile;
}