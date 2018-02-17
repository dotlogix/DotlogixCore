// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DiffCollection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Collections.Generic;
using System.Linq;
#endregion

namespace DotLogix.Core.Utils {
    public class DiffCollection<T> {
        public IEnumerable<T> LeftOnly { get; }
        public IEnumerable<T> Intersect { get; }
        public IEnumerable<T> RightOnly { get; }
        public IEnumerable<T> Union { get; }

        public DiffCollection(IEnumerable<T> left, IEnumerable<T> right, IEqualityComparer<T> comparer) {
            var leftHashset = new HashSet<T>(left, comparer);
            var rightHashset = new HashSet<T>(right, comparer);

            var intersectHashset = new HashSet<T>(leftHashset);

            intersectHashset.IntersectWith(rightHashset);
            leftHashset.ExceptWith(intersectHashset);
            rightHashset.ExceptWith(intersectHashset);
            LeftOnly = leftHashset.ToList();
            RightOnly = rightHashset.ToList();
            Intersect = intersectHashset.ToList();
            Union = LeftOnly.Concat(Intersect).Concat(RightOnly).ToList();
        }
    }
}
