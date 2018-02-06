using System;

namespace DotLogix.Core.Caching
{

    public interface ICache<TKey> : IDisposable{
        TimeSpan CheckPolicyInterval { get; }
        object this[TKey key] { get; }
        bool Contains(TKey key);

        void Store(TKey key, object value, ICachePolicy policy = null);
        TValue Retrieve<TValue>(TKey key);
        object Retrieve(TKey key);
        TValue Pop<TValue>(TKey key);
        object Pop(TKey key);
        bool Remove(TKey key);
        bool TryRetrieve<TValue>(TKey key, out TValue value);
        bool TryRetrieve(TKey key, out object value);
    }
}
