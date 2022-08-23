#region
using System.Collections.Generic;
using System.Linq;
using DotLogix.Core.Utils.Mappers;
#endregion

namespace DotLogix.Core.Extensions {
    public static class MapperExtensions {
        public static TTarget Map<TSource, TTarget>(this IMapper<TSource, TTarget> mapper, TSource source) where TTarget : new() {
            var target = new TTarget();
            mapper.Map(source, target);
            return target;
        }

        public static IEnumerable<TTarget> Map<TSource, TTarget>(this IMapper<TSource, TTarget> mapper, IEnumerable<TSource> source) where TTarget : new() {
            return source.Select(mapper.Map);
        }
    }
}