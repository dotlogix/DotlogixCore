using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DotLogix.WebServices.EntityFramework.Context;
using DotLogix.WebServices.EntityFramework.Extensions;

namespace DotLogix.WebServices.EntityFramework.Utils.Sql; 

public sealed class HierarchySqlCteBuilder<TResult, TKey> : SqlCteBuilder<TResult, IHierarchySqlCteBuilder<TResult, TKey>>, IHierarchySqlCteBuilder<TResult, TKey>
    where TResult : class
{
    private Expression<Func<TResult, TKey>> _currentKeySelector;
    private Expression<Func<TResult, TKey>> _nextKeySelector;

    private Expression<Func<TResult, bool>> _initialFilter;
    private Expression<Func<TResult, bool>> _nextFilter;
    private Expression<Func<TResult, bool>> _resultFilter;
    private bool _excludeInitial;

    public HierarchySqlCteBuilder(IEntityContext entityContext, IReadOnlyDictionary<string, ISqlCte> references) : base(entityContext, references) {
    }

    public IHierarchySqlCteBuilder<TResult, TKey> UseKey(Expression<Func<TResult, TKey>> selector) {
        _currentKeySelector = selector;
        return this;
    }

    public IHierarchySqlCteBuilder<TResult, TKey> UseNextKey(Expression<Func<TResult, TKey>> selector) {
        _nextKeySelector = selector;
        return this;
    }

    public IHierarchySqlCteBuilder<TResult, TKey> UseInitialFilter(Expression<Func<TResult, bool>> filter) {
        _initialFilter = filter;
        return this;
    }

    public IHierarchySqlCteBuilder<TResult, TKey> UseNextFilter(Expression<Func<TResult, bool>> filter) {
        _nextFilter = filter;
        return this;
    }

    public IHierarchySqlCteBuilder<TResult, TKey> UseResultFilter(Expression<Func<TResult, bool>> filter) {
        _resultFilter = filter;
        return this;
    }
    
    public IHierarchySqlCteBuilder<TResult, TKey> ExcludeInitial(bool exclude = true) {
        _excludeInitial = exclude;
        return this;
    }

    protected override IQueryable<TResult> BuildQuery() {
        var initialQuery = EntityContext.Query<TResult>();
        if(_initialFilter is not null) {
            initialQuery = initialQuery.Where(_initialFilter);
        }

        var nextQuery = EntityContext.Query<TResult>();
        if(_nextFilter is not null) {
            nextQuery = nextQuery.Where(_nextFilter);
        }

        nextQuery = nextQuery.Join(
            RecursiveReference,
            _currentKeySelector,
            _nextKeySelector,
            (next, _) => next
        );
        var resultQuery = initialQuery.Concat(nextQuery);
        if(_resultFilter is not null) {
            resultQuery = resultQuery.Where(_resultFilter);
        }
        
        if(_initialFilter is not null && _excludeInitial) {
            var invertedInitialFilter = _initialFilter.Update(Expression.Not(_initialFilter.Body), _initialFilter.Parameters);
            resultQuery = resultQuery.Where(invertedInitialFilter);
        }
        return resultQuery;
    }
}