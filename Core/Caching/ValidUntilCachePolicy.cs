using System;

namespace DotLogix.Core.Caching {
    public class ValidUntilCachePolicy : ICachePolicy {
        private readonly DateTime _validUntilUtc;

        public ValidUntilCachePolicy(TimeSpan duration) : this(DateTime.UtcNow + duration){
            
        }

        public ValidUntilCachePolicy(DateTime validUntilUtc) {
            _validUntilUtc = validUntilUtc;
        }

        public bool HasExpired(DateTime timeStampUtc) {
            return timeStampUtc < _validUntilUtc;
        }
    }
}