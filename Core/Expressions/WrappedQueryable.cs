// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  WrappedQueryable.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 10.05.2021 22:49
// LastEdited:  26.09.2021 22:15
// ==================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DotLogix.Core.Expressions {
    public class WrappedQueryable<T> : IQueryable<T>, IOrderedQueryable<T> {
        private IQueryable<T> _innerQueryable;

        public Type ElementType => _innerQueryable.ElementType;
        public Expression Expression => _innerQueryable.Expression;
        public IQueryProvider Provider { get; }

        public WrappedQueryable(IQueryProvider provider, IQueryable<T> innerQueryable) {
            Provider = provider;
            _innerQueryable = innerQueryable;
        }

        public IEnumerator<T> GetEnumerator() {
            return _innerQueryable.Provider.CreateQuery<T>(Expression).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)_innerQueryable).GetEnumerator();
        }
    }
}