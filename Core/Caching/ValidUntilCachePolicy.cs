// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  ValidUntilCachePolicy.cs
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
    public class ValidUntilCachePolicy : ICachePolicy {
        private readonly DateTime _validUntilUtc;

        /// <summary>
        /// Creates a new instance of <see cref="ValidUntilCachePolicy"/>
        /// </summary>
        /// <param name="duration">The duration until the item expires</param>
        public ValidUntilCachePolicy(TimeSpan duration) : this(DateTime.UtcNow + duration) { }

        /// <summary>
        /// Creates a new instance of <see cref="ValidUntilCachePolicy"/>
        /// </summary>
        /// <param name="validUntilUtc">The time when the item has expired</param>
        public ValidUntilCachePolicy(DateTime validUntilUtc) {
            _validUntilUtc = validUntilUtc;
        }

        /// <inheritdoc />
        public bool HasExpired(DateTime timeStampUtc) {
            return timeStampUtc > _validUntilUtc;
        }
    }
}
