// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IOrderedQuery.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Linq.Expressions;
#endregion

namespace DotLogix.Architecture.Infrastructure.Queries {
    /// <summary>
    /// An ordered extension of the <see cref="IQuery{T}"/> interface
    /// </summary>
    public interface IOrderedQuery<T> : IQuery<T> {
        /// <summary>
        /// Sorts the query by another key if the previous one is equal
        /// </summary>
        IOrderedQuery<T> ThenBy<TKey>(Expression<Func<T, TKey>> keySelector);
        /// <summary>
        /// Sorts the query by another key in descending order if the previous one is equal
        /// </summary>
        IOrderedQuery<T> ThenByDescending<TKey>(Expression<Func<T, TKey>> keySelector);
    }
}
