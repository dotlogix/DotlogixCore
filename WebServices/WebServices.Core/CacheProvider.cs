// ==================================================
// Copyright 2019(C) , DotLogix
// File:  CacheConstants.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  16.07.2018
// LastEdited:  20.01.2019
// ==================================================

#region
 using System;
 using System.Collections.Generic;
 using DotLogix.Core.Caching;
 using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.WebServices.Core; 

public class CacheProvider : ICacheProvider {
    private readonly Dictionary<string, Cache<object, object>> _cacheDict = new Dictionary<string, Cache<object, object>>(StringComparer.OrdinalIgnoreCase);

    public Cache<object, object> Get(string name) => _cacheDict.GetValueOrDefault(name);

    public Cache<object, object> GetOrCreate(string name, TimeSpan delay) {
        return _cacheDict.GetOrAdd(name, () => new Cache<object,object>(delay));
    }
        
    public Cache<object, object> Replace(string name, TimeSpan delay) {
        var cache = new Cache<object,object>(delay);
        _cacheDict[name] = cache;
        return cache;
    }
        
    public Cache<object, object> Remove(string name) {
        return _cacheDict.TryPopValue(name, out var cache) ? cache : null;
    }

    public void Clear(string name) {
        Get(name)?.Clear();
    }
        
    public void Clear() {
        foreach(var cache in _cacheDict.Values) {
            cache.Clear();
        }
    }
}