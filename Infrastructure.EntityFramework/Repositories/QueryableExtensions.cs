using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DotLogix.Architecture.Infrastructure.EntityFramework.Repositories {
    public static class QueryableExtensions{
        public static Task<IEnumerable<TSource>> ToEnumerableAsync<TSource>(this IQueryable<TSource> queryable) {
            return ToEnumerableAsync(queryable, CancellationToken.None);
        }
        public static Task<IEnumerable<TSource>> ToEnumerableAsync<TSource>(this IQueryable<TSource> queryable, CancellationToken cancellationToken) {
            return queryable.ToListAsync(cancellationToken).ConvertResult<IEnumerable<TSource>, List<TSource>>();
        }
    }
}