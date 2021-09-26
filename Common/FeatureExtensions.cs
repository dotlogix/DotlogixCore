using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Common.Features;

namespace DotLogix.Common {
    public static class FeatureExtensions {
        public static Guid EnsureGuid(this IGuid value) {
            if(value.Guid == Guid.Empty)
                return value.Guid = Guid.NewGuid();
            return value.Guid;
        }
        
        public static HashSet<Guid> GetUniqueKeys<T>(this IEnumerable<T> values) where T : IGuid
        {
            var keys = new HashSet<Guid>(values.Select(p => p.Guid));
            keys.Remove(Guid.Empty);
            return keys;
        }
    }
}
