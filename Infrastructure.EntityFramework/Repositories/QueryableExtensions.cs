// ==================================================
// Copyright 2018(C) , DotLogix
// File:  QueryableExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using Microsoft.EntityFrameworkCore;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityFramework.Repositories {
    public static class QueryableExtensions {
        public static Task<IEnumerable<TSource>> ToEnumerableAsync<TSource>(this IQueryable<TSource> queryable) {
            return ToEnumerableAsync(queryable, CancellationToken.None);
        }

        public static Task<IEnumerable<TSource>> ToEnumerableAsync<TSource>(this IQueryable<TSource> queryable, CancellationToken cancellationToken) {
            return queryable.ToListAsync(cancellationToken).ConvertResult<IEnumerable<TSource>, List<TSource>>();
        }
    }
}
