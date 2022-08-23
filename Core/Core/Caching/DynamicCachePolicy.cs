// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  DynamicCachePolicy.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 22.08.2020 13:51
// LastEdited:  26.09.2021 22:27
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Caching {
    /// <summary>
    ///     A cache policy where values are valid until a given timestamp
    /// </summary>
    public class DynamicCachePolicy : ICachePolicy {
        private readonly Func<DateTime, bool> _hasExpiredFunc;

        /// <summary>
        /// Creates a new instance of <see cref="DynamicCachePolicy"/>
        /// </summary>
        /// <param name="hasExpiredFunc">The callback to get the expiration status</param>
        public DynamicCachePolicy(Func<DateTime, bool> hasExpiredFunc) {
            _hasExpiredFunc = hasExpiredFunc;
        }
        
        /// <summary>
        /// Creates a new instance of <see cref="DynamicCachePolicy"/>
        /// </summary>
        /// <param name="hasExpiredFunc">The callback to get the expiration status</param>
        public DynamicCachePolicy(Func<bool> hasExpiredFunc) {
            _hasExpiredFunc = (_) => hasExpiredFunc.Invoke();
        }


        /// <inheritdoc />
        public bool HasExpired(DateTime timeStampUtc) {
            return _hasExpiredFunc.Invoke(timeStampUtc);
        }
    }
}