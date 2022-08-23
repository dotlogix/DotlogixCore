using System;
using System.Linq.Expressions;

namespace DotLogix.WebServices.EntityFramework.Utils.Sql; 

public interface IHierarchySqlCteBuilder<TResult, TKey> : ISqlCteBuilder<IHierarchySqlCteBuilder<TResult, TKey>>
    where TResult : class
{
    IHierarchySqlCteBuilder<TResult, TKey> UseKey(Expression<Func<TResult, TKey>> selector);
    IHierarchySqlCteBuilder<TResult, TKey> UseNextKey(Expression<Func<TResult, TKey>> selector);
    
    IHierarchySqlCteBuilder<TResult, TKey> UseInitialFilter(Expression<Func<TResult, bool>> filter);
    IHierarchySqlCteBuilder<TResult, TKey> UseNextFilter(Expression<Func<TResult, bool>> filter);
    IHierarchySqlCteBuilder<TResult, TKey> UseResultFilter(Expression<Func<TResult, bool>> filter);
    
    IHierarchySqlCteBuilder<TResult, TKey> ExcludeInitial(bool exclude = true);
}