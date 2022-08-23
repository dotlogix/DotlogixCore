using System;
using DotLogix.Core.Caching;

namespace DotLogix.WebServices.Core; 

public interface ICacheProvider {
    Cache<object, object> Get(string name);
    Cache<object, object> GetOrCreate(string name, TimeSpan delay);
    Cache<object, object> Replace(string name, TimeSpan delay);
    Cache<object, object> Remove(string name);
    void Clear(string name);
    void Clear();
}