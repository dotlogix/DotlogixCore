using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.WebServices.EntityFramework.Context;
using DotLogix.WebServices.EntityFramework.Extensions;

namespace DotLogix.WebServices.EntityFramework.Utils.Sql; 

public abstract class SqlCteBuilder<TResult, TBuilder> : ISqlCteBuilder<TBuilder>
    where TResult : class
    where TBuilder : ISqlCteBuilder<TBuilder>
{
    private IQueryable<TResult> _reference;
    
    protected IEntityContext EntityContext { get; }
    protected IReadOnlyDictionary<string, ISqlCte> References { get; }

    protected string Name { get; set; }
    protected string Alias { get; set; }
    protected bool? IsMaterialized { get; set; }
    protected bool IsRecursive { get; private set; }
    protected IQueryable<TResult> RecursiveReference {
        get {
            EnsureRecursive();
            return _reference;
        }
    }

    protected SqlCteBuilder(IEntityContext entityContext, IReadOnlyDictionary<string, ISqlCte> references) {
        EntityContext = entityContext;
        References = references;
    }

    public TBuilder UseName(string name) {
        Name = name;
        return (TBuilder)(object)this;
    }

    public TBuilder UseMaterialize(bool? materialize = true) {
        IsMaterialized = materialize;
        return (TBuilder)(object)this;
    }

    public ISqlCte Build() {
        return new SqlCte {
            Type = typeof(TResult),
            Name = Name,
            Alias = Alias,
            Recursive = IsRecursive,
            Materialize = IsMaterialized,
            Query = BuildQuery(),
            Reference = RecursiveReference
        };
    }

    protected abstract IQueryable<TResult> BuildQuery();

    private void EnsureRecursive() {
        if(IsRecursive) {
            return;
        }

        IsRecursive = true;
        Alias = $"<<{Guid.NewGuid():N}>>";
        _reference = EntityContext.Query<TResult>("SELECT * FROM {Alias}");
    }
}