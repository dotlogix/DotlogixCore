// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ValidUntilCachePolicy.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Caching {
    public class ValidUntilCachePolicy : ICachePolicy {
        private readonly DateTime _validUntilUtc;

        public ValidUntilCachePolicy(TimeSpan duration) : this(DateTime.UtcNow + duration) { }

        public ValidUntilCachePolicy(DateTime validUntilUtc) {
            _validUntilUtc = validUntilUtc;
        }

        public bool HasExpired(DateTime timeStampUtc) {
            return timeStampUtc > _validUntilUtc;
        }
    }
}
