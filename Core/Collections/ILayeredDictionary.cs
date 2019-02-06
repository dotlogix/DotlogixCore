// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ILayeredDictionary.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.12.2018
// LastEdited:  13.12.2018
// ==================================================

#region
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Collections {
    public interface ILayeredDictionary<TKey, TValue, TDictionary> : ILayeredCollection<KeyValuePair<TKey, TValue>, TDictionary>, IDictionary<TKey, TValue> where TDictionary : IDictionary<TKey, TValue> { }
}
