using System.Linq.Expressions;

namespace DotLogix.WebServices.EntityFramework.Database; 

public interface IEvaluatableFilter {
    bool IsEvaluatable(Expression expression);
}