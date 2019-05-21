// ==================================================
// Copyright 2019(C) , DotLogix
// File:  JsonWebTokens.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.01.2019
// LastEdited:  03.01.2019
// ==================================================

#region
using System;
using System.Linq;
using System.Text;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes;
using DotLogix.Core.Nodes.Processor;
using DotLogix.Core.Rest.Authentication.Jwt.Algorithms;
#endregion

namespace DotLogix.Core.Rest.Authentication.Jwt {
    public static class JsonWebTokens {
        public static string Serialize<TPayload>(this JsonWebToken<TPayload> token, ISigningAlgorithm signingAlgorithm) {
            var header = token.Header.Clone();
            header.AddOrReplaceChild("alg", new NodeValue(signingAlgorithm.Name.ToUpper()));
            header.AddOrReplaceChild("typ", new NodeValue("JWT"));

            var headerStr = StringExtensions.EncodeBase64Url(header.ToJson());
            var payloadStr = StringExtensions.EncodeBase64Url(JsonNodes.ToJson(token.Payload, JsonFormatterSettings.Default));
            var tokenStr = string.Concat(headerStr, ".", payloadStr);

            var signatureStr = StringExtensions.EncodeBase64Url(signingAlgorithm.CalculateSignature(Encoding.UTF8.GetBytes(tokenStr)));
            tokenStr = string.Concat(tokenStr, ".", signatureStr);
            return tokenStr;
        }

        public static JsonWebToken<TPayload> Deserialize<TPayload>(string data, params ISigningAlgorithm[] signingAlgorithms) {
            return Deserialize<TPayload>(data, name => FindAlgorithm(name, signingAlgorithms));
        }

        public static JsonWebToken<TPayload> Deserialize<TPayload>(string data, Func<string, ISigningAlgorithm> resolveAlgorithmFunc) {
            var result = TryDeserialize<TPayload>(data, out var token, resolveAlgorithmFunc);
            if(result == JsonWebTokenResult.Success)
                return token;
            throw new JsonWebTokenException("The provided token is not valid, see result for more information", result);
        }

        public static JsonWebTokenResult TryDeserialize<TPayload>(string data, out JsonWebToken<TPayload> token, params ISigningAlgorithm[] signingAlgorithms) {
            return TryDeserialize(data, out token, name => FindAlgorithm(name, signingAlgorithms));
        }

        public static JsonWebTokenResult TryDeserialize<TPayload>(string data, out JsonWebToken<TPayload> token, Func<string, ISigningAlgorithm> resolveAlgorithmFunc) {
            token = null;
            var split = data.Split('.');

            if(split.Length != 3)
                return JsonWebTokenResult.InvalidFormat;

            var headerStr = split[0];
            var payloadStr = split[1];
            var signatureStr = split[2];

            var headerNode = JsonNodes.ToNode<NodeMap>(StringExtensions.DecodeBase64Url(headerStr));
            if((headerNode.TryGetChildValue("typ", out string typ) == false) || (!string.Equals(typ, "jwt", StringComparison.OrdinalIgnoreCase)))
                return JsonWebTokenResult.InvalidType;

            ISigningAlgorithm signingAlgorithm;
            if((headerNode.TryGetChildValue("alg", out string algorithm) == false) || ((signingAlgorithm = resolveAlgorithmFunc.Invoke(algorithm)) == null))
                return JsonWebTokenResult.InvalidAlgorithm;

            var unsignedData = Encoding.UTF8.GetBytes(data.Substring(0, headerStr.Length + payloadStr.Length + 1));
            var signature = StringExtensions.DecodeBase64UrlToArray(signatureStr);
            if(signingAlgorithm.ValidateSignature(unsignedData, signature) == false)
                return JsonWebTokenResult.InvalidSignature;

            token = new JsonWebToken<TPayload>(headerNode, JsonNodes.FromJson<TPayload>(StringExtensions.DecodeBase64Url(payloadStr)));
            return JsonWebTokenResult.Success;
        }

        private static ISigningAlgorithm FindAlgorithm(string name, ISigningAlgorithm[] signingAlgorithms) {
            return signingAlgorithms.FirstOrDefault(a => string.Equals(name, a.Name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
