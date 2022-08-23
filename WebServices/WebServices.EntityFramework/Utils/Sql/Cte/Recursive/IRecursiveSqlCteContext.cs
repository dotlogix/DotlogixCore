using System.Linq;

namespace DotLogix.WebServices.EntityFramework.Utils.Sql; 

public interface IRecursiveSqlCteContext<out T> : ISqlCteContext {
    IQueryable<T> Current();
}