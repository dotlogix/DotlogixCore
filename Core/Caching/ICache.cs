// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ICache.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Caching {
    public interface ICache<TKey, TValue> : IDisposable {
        TimeSpan CheckPolicyInterval { get; }
        TValue this[TKey key] { get; }


        bool IsAlive(TKey key);

        void Store(TKey key, TValue value, ICachePolicy policy = null);

        TValue Retrieve(TKey key);
        bool TryRetrieve(TKey key, out TValue value);

        TValue Pop(TKey key);
        bool TryPop(TKey key, out TValue value);

        bool Discard(TKey key);
        void Cleanup();
    }
}
