// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DiffEnumerable.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Utils {
    /// <summary>
    /// A class to diff two enumerable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DiffEnumerable<T> {
        /// <summary>
        /// Values only occuring left
        /// </summary>
        public IEnumerable<T> LeftOnly { get; }
        /// <summary>
        /// Values occuring in both
        /// </summary>
        public IEnumerable<T> Intersect { get; }
        /// <summary>
        /// Values only occuring right
        /// </summary>
        public IEnumerable<T> RightOnly { get; }

        /// <summary>
        /// Creates a new instance of <see cref="DiffEnumerable{T}"/>
        /// </summary>
        public DiffEnumerable(IEnumerable<T> leftOnly, IEnumerable<T> intersect, IEnumerable<T> rightOnly) {
            LeftOnly = leftOnly;
            Intersect = intersect;
            RightOnly = rightOnly;
        }
    }

    /// <summary>
    /// A class to diff two enumerable
    /// </summary>
    public class DiffEnumerable<TLeft, TRight> {
        /// <summary>
        /// Values only occuring left
        /// </summary>
        public IReadOnlyCollection<TLeft> LeftOnly { get; }
        /// <summary>
        /// Values occuring in both
        /// </summary>
        public IReadOnlyCollection<DiffValue> Intersect { get; }
        /// <summary>
        /// Values only occuring right
        /// </summary>
        public IReadOnlyCollection<TRight> RightOnly { get; }

        /// <summary>
        /// Creates a new instance of <see cref="DiffEnumerable{TLeft, TRight}"/>
        /// </summary>
        public DiffEnumerable(IReadOnlyCollection<TLeft> leftOnly, IReadOnlyCollection<DiffValue> intersect, IReadOnlyCollection<TRight> rightOnly) {
            LeftOnly = leftOnly;
            Intersect = intersect;
            RightOnly = rightOnly;
        }

        /// <summary>
        /// A tuple containing the left and right values of a diff
        /// </summary>
        public class DiffValue {
            /// <summary>
            /// Left
            /// </summary>
            public TLeft Left { get; }
            /// <summary>
            /// Right
            /// </summary>
            public TRight Right { get; }

            /// <summary>
            /// Creates a new instance of <see cref="DiffValue"/>
            /// </summary>
            public DiffValue(TLeft left, TRight right) {
                Left = left;
                Right = right;
            }

            /// <summary>
            /// Deconstructs the value in their parts
            /// </summary>
            public void Deconstruct(out TLeft left, out TRight right) {
                left = Left;
                right = Right;
            }
        }
    }
}