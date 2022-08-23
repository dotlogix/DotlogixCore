using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Common.Features;

namespace DotLogix.Common; 

public static class FeatureExtensions {
    public static Guid EnsureGuid(this IGuid value) {
        if(value.Guid == Guid.Empty) {
            return value.Guid = Guid.NewGuid();
        }

        return value.Guid;
    }

    public static HashSet<Guid> GetUniqueKeys<T>(this IEnumerable<T> values) where T : IGuid {
        return GetUniqueKeys(values, e => e.Guid);
    }

    public static HashSet<TKey> GetUniqueKeys<T, TKey>(this IEnumerable<T> values, Func<T, TKey> keySelector) where T : IGuid {
        var keys = new HashSet<TKey>(values.Select(keySelector));
        keys.Remove(default);
        return keys;
    }
}