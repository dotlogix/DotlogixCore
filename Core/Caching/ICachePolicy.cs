using System;

namespace DotLogix.Core.Caching {
    public interface ICachePolicy {
        bool HasExpired(DateTime timeStampUtc);
    }
}