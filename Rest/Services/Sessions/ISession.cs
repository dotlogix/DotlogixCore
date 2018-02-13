using System;
using System.Collections.Generic;

namespace DotLogix.Core.Rest.Services.Sessions {
    public interface ISession {
        Guid Token { get; }
        DateTime ValidUntilUtc { get; }

        void Renew(DateTime validUntilUtc);
    }
}