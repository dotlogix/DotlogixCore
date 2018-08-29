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
using System.Text;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Services.Processors.Authentication.Base;
using DotLogix.Core.Rest.Services.Processors.Authentication.Jwt.Algorithms;
#endregion

namespace DotLogix.Core.Rest.Services.Processors.Authentication.Jwt {
    public class JwtAuthenticationMethod<TClaim> : AuthenticationMethodBase {
        private readonly Dictionary<string, ISigningAlgorithm> _algorithms;
        private readonly ValidateClaimAsyncCallback<TClaim> _callbackAsync;

        public JwtAuthenticationMethod(ValidateClaimAsyncCallback<TClaim> callbackAsync, params ISigningAlgorithm[] algorithms) : this(callbackAsync, algorithms.AsEnumerable()) { }

        public JwtAuthenticationMethod(ValidateClaimAsyncCallback<TClaim> callbackAsync, IEnumerable<ISigningAlgorithm> algorithms) : base("Bearer", "[token] in [header:base64].[payload:base64].[signature:base64] format") {
            _callbackAsync = callbackAsync;
            _algorithms = algorithms.ToDictionary(a => a.Name, StringComparer.OrdinalIgnoreCase);
        }

        public override Task AuthenticateAsync(WebRequestResult webRequestResult, string data) {
            var split = data.Split('.');

            if(split.Length != 3) {
                SetInvalidFormatException(webRequestResult);
                return Task.CompletedTask;
            }


            var headerStr = split[0];
            var claimStr = split[1];
            var signatureStr = split[2];

            var headerNode = JsonNodes.ToNode<NodeMap>(StringExtensions.DecodeBase64Url(headerStr));
            if((headerNode.TryGetChildValue("typ", out string typ) == false) || (typ != "jwt") || (headerNode.TryGetChildValue("alg", out string algorithm) == false) || (_algorithms.TryGetValue(algorithm, out var signingAlgorithm) == false)) {
                SetInvalidFormatException(webRequestResult);
                return Task.CompletedTask;
            }

            var unsignedData = Encoding.UTF8.GetBytes(data.Substring(0, headerStr.Length + claimStr.Length + 1));
            var signature = StringExtensions.DecodeBase64UrlToArray(signatureStr);
            if(signingAlgorithm.ValidateSignature(unsignedData, signature) == false) {
                SetUnauthorizedException(webRequestResult, "The signature does not match");
                return Task.CompletedTask;
            }

            var claim = JsonNodes.FromJson<TClaim>(StringExtensions.DecodeBase64Url(claimStr));
            return _callbackAsync.Invoke(this, webRequestResult, claim);
        }
    }
}
