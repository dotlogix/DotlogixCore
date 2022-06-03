// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  CacheItemDiscardReason.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 22.08.2020 13:51
// LastEdited:  26.09.2021 22:27
// ==================================================

namespace DotLogix.Core.Caching {
    /// <summary>
    /// Some reasons why a cache item was discarded
    /// </summary>
    public enum CacheItemDiscardReason {
        /// <summary>
        /// The discard reason is unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// The item was discarded by expiration
        /// </summary>
        Expired = 1,
        /// <summary>
        /// The item was discarded by user action
        /// </summary>
        Discarded = 2,
    }
}