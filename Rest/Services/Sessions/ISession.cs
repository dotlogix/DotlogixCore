using System;
using System.Collections.Generic;

namespace DotLogix.Core.Rest.Services.Sessions {
    public interface ISession {
        Guid Token { get; }
    }

    public class Session : ISession {
        public Session(Guid token) {
            Token = token;
        }
        public Guid Token { get; }
    }
}