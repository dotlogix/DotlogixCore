// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DiffCollection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Utils {
    public class DiffEnumerable<T> {
        public IEnumerable<T> LeftOnly { get; }
        public IEnumerable<T> Intersect { get; }
        public IEnumerable<T> RightOnly { get; }

        public DiffEnumerable(IEnumerable<T> leftOnly, IEnumerable<T> intersect, IEnumerable<T> rightOnly) {
            LeftOnly = leftOnly;
            Intersect = intersect;
            RightOnly = rightOnly;
        }
    }

    

    public class DiffEnumerable<TLeft, TRight>
    {
        public IEnumerable<TLeft> LeftOnly { get; }
        public IEnumerable<DiffValue> Intersect { get; }
        public IEnumerable<TRight> RightOnly { get; }

        public DiffEnumerable(IEnumerable<TLeft> leftOnly, IEnumerable<DiffValue> intersect, IEnumerable<TRight> rightOnly)
        {
            LeftOnly = leftOnly;
            Intersect = intersect;
            RightOnly = rightOnly;
        }
        public class DiffValue
        {
            public DiffValue(TLeft left, TRight right)
            {
                Left = left;
                Right = right;
            }
            public TLeft Left { get; }
            public TRight Right { get; }
        }
    }

    
}
