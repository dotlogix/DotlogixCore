﻿using System;
using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Services.Processors.Authentication.Base;

namespace DotLogix.Core.Rest.Services.Processors.Authentication.Token {
    public class TokenAuthenticationMethod : AuthenticationMethodBase
    {
        public TokenAuthenticationMethod(ValidateTokenAsyncCallback callbackAsync) : base("Bearer", "[token] in guid:d (00000000-0000-0000-0000-000000000000) format") {
            _callbackAsync = callbackAsync;
        }

        private readonly ValidateTokenAsyncCallback _callbackAsync;

        public override Task AuthenticateAsync(WebRequestResult webRequestResult, string data)
        {
            if(Guid.TryParseExact(data, "D", out var token))
                return _callbackAsync.Invoke(this, webRequestResult, token);

            SetInvalidFormatException(webRequestResult);
            return Task.CompletedTask;
        }
    }
}