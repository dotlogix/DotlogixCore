// ==================================================
// Copyright 2018(C) , DotLogix
// File:  TokenAuthenticationMethod.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Threading.Tasks;
using DotLogix.Core.Rest.Authentication.Base;
using DotLogix.Core.Rest.Server.Http;
#endregion

namespace DotLogix.Core.Rest.Authentication.Token {
    public class TokenAuthenticationMethod : AuthenticationMethodBase {
        private readonly ValidateTokenAsyncCallback _callbackAsync;

        public TokenAuthenticationMethod(ValidateTokenAsyncCallback callbackAsync) : base("Bearer", "[token] in guid:d (00000000-0000-0000-0000-000000000000) format") {
            _callbackAsync = callbackAsync;
        }

        public override Task AuthenticateAsync(WebRequestResult webRequestResult, string data) {
            if(Guid.TryParseExact(data, "D", out var token))
                return _callbackAsync.Invoke(this, webRequestResult, token);

            SetInvalidFormatException(webRequestResult);
            return Task.CompletedTask;
        }
    }
}
