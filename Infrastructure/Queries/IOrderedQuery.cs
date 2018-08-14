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
    public interface IOrderedQuery<T> : IQuery<T> {
        IOrderedQuery<T> ThenBy<TKey>(Expression<Func<T, TKey>> keySelector);
        IOrderedQuery<T> ThenByDescending<TKey>(Expression<Func<T, TKey>> keySelector);
    }
}
