using System.Linq.Expressions;

namespace DotLogix.WebServices.EntityFramework.Database; 

public interface IEntityQueryRewriterPlugin {
    Expression Rewrite(Expression expression);
}