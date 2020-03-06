// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ICachePolicy.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
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
