using System.Linq.Expressions;

namespace DotLogix.WebServices.EntityFramework.Database {
    public interface IEntityQueryRewriter {
        Expression Rewrite(Expression expression);
    }
}