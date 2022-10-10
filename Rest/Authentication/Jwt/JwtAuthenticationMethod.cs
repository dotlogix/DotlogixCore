// ==================================================
// Copyright 2018(C) , DotLogix
// File:  TokenAuthenticationMethod.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  28.08.2018
// LastEdited:  28.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes;
using DotLogix.Core.Rest.Authentication.Base;
using DotLogix.Core.Rest.Authentication.Jwt.Algorithms;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Services;
using DotLogix.Core.Rest.Services.Context;
using DotLogix.Core.Rest.Services.Writer;
#endregion

namespace DotLogix.Core.Rest.Authentication.Jwt {
    public class JwtAuthenticationMethod<TPayload> : AuthenticationMethodBase {
        private readonly Dictionary<string, ISigningAlgorithm> _signingAlgorithms;
        private readonly ValidateClaimAsyncCallback<TPayload> _callbackAsync;

        public JwtAuthenticationMethod(ValidateClaimAsyncCallback<TPayload> callbackAsync, params ISigningAlgorithm[] algorithms) : this(callbackAsync, algorithms.AsEnumerable()) { }

        public JwtAuthenticationMethod(ValidateClaimAsyncCallback<TPayload> callbackAsync, IEnumerable<ISigningAlgorithm> algorithms) : base("Bearer", "[token] in [header:base64].[payload:base64].[signature:base64] format") {
            _callbackAsync = callbackAsync;
            _signingAlgorithms = algorithms.ToDictionary(a => a.Name, StringComparer.OrdinalIgnoreCase);
        }

        public override Task AuthenticateAsync(WebServiceContext context, string data) {
            var formatterSettings = context.Settings.Get(WebServiceSettings.JsonFormatterSettings, JsonFormatterSettings.Idented);

            var result = JsonWebTokens.TryDeserialize<TPayload>(data, out var token, name => _signingAlgorithms.GetValue(name), formatterSettings);
            if(result == JsonWebTokenResult.Success)
                return _callbackAsync.Invoke(this, context, token.Payload);
            SetUnauthorizedException(context, $"Token could not be validated ({result})");
            return Task.CompletedTask;
        }
    }
}
