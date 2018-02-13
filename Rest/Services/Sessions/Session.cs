using System;

namespace DotLogix.Core.Rest.Services.Sessions {
    public class Session : ISession {
        public Session(Guid token) {
            Token = token;
        }
        public Guid Token { get; }
        public DateTime ValidUntilUtc { get; private set; }

        public void Renew(DateTime validUntilUtc) {
            ValidUntilUtc = validUntilUtc;
        }
    }
}