#region
using System;
using System.Collections.Generic;
#endregion

namespace DotLogix.WebServices.Core.Terms {
    public class PaginationResult<TResult> {
        public int Page { get; set; }
        public int Count => Values.Count;
        public int PageSize { get; set; }

        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public int TotalCount { get; set; }
        public ICollection<TResult> Values { get; set; }
        public ManyTerm<OrderingKey> OrderBy { get; set; }
    }
}
