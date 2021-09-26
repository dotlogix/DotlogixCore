// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  CacheItemDiscardSource.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 22.08.2020 13:51
// LastEdited:  26.09.2021 22:27
// ==================================================

namespace DotLogix.Core.Caching {
    /// <summary>
    /// Some reasons why a cache item was discarded
    /// </summary>
    public enum CacheItemDiscardSource {
        /// <summary>
        /// The discard source is unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// The item itself was discarded
        /// </summary>
        Self = 1,
        /// <summary>
        /// The item was discarded as a dependency of a parent
        /// </summary>
        Ancestor = 2,
    }
}