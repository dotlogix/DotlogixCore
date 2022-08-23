using System.Collections.Generic;
using System.Linq.Expressions;
using DotLogix.Core.Extensions;
using Microsoft.EntityFrameworkCore.Query;

namespace DotLogix.WebServices.EntityFramework.Database; 

public class EntityEvaluatableFilterPlugin : IEvaluatableExpressionFilterPlugin {
    private readonly IReadOnlyCollection<IEvaluatableFilter> _filters;

    public EntityEvaluatableFilterPlugin(IEnumerable<IEvaluatableFilter> filters) {
        _filters = filters.AsReadOnlyCollection();
    }

    public bool IsEvaluatableExpression(Expression expression) {
        if(_filters.Count == 0) {
            return true;
        }
        foreach(var filter in _filters) {
            if(filter.IsEvaluatable(expression) == false)
                return false;
        }
        return true;
    }
}