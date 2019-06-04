// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ValidUntilCachePolicy.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
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
        /// Creates an instance of <see cref="ValidUntilCachePolicy"/>
        /// </summary>
        /// <param name="duration">The duration until the item expires</param>
        public ValidUntilCachePolicy(TimeSpan duration) : this(DateTime.UtcNow + duration) { }

        /// <summary>
        /// Creates an instance of <see cref="ValidUntilCachePolicy"/>
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
