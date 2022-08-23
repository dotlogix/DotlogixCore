// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  ICachePolicy.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 22.08.2020 13:51
// LastEdited:  26.09.2021 22:27
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Caching {
    /// <summary>
    ///     A common interface for cache policies
    /// </summary>
    public interface ICachePolicy {
        /// <summary>
        /// A callback method to check if an item should be dropped
        /// </summary>
        /// <param name="timeStampUtc">The timestamp when the check is happening</param>
        /// <returns></returns>
        bool HasExpired(DateTime timeStampUtc);
    }
}