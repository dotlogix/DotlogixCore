// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  INamedCollection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 22.08.2020 13:51
// LastEdited:  26.09.2021 22:27
// ==================================================

using System.Collections.Generic;

namespace DotLogix.Core.Collections {
    /// <summary>
    /// An interface to represent dynamic settings
    /// </summary>
    public interface INamedCollection : IReadOnlyNamedCollection {
        /// <summary>
        /// Adds or updates a setting with the corresponding key
        /// </summary>
        void Set(string key, object value = default);

        /// <summary>
        /// Removes the setting to the key
        /// </summary>
        /// <returns>true if key was removed, otherwise false</returns>
        bool Reset(string key);

        /// <summary>
        /// Sets a number of settings based on the values and their keys
        /// </summary>
        void Apply(IEnumerable<KeyValuePair<string, object>> values, bool replaceExisting = true);
    }
}