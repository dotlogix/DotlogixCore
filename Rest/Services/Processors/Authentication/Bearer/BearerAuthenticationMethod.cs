// ==================================================
// Copyright 2018(C) , DotLogix
// File:  BearerAuthenticationMethod.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Services.Processors.Authentication.Base;
#endregion

namespace DotLogix.Core.Rest.Services.Processors.Authentication.Bearer {
    public class BearerAuthenticationMethod : AuthenticationMethodBase {
        private readonly ValidateBearerAsyncCallback _callbackAsync;


        public BearerAuthenticationMethod(ValidateBearerAsyncCallback callbackAsync, params string[] supportedDataFormats) : base("Bearer", supportedDataFormats) {
            _callbackAsync = callbackAsync;
        }

        public BearerAuthenticationMethod(ValidateBearerAsyncCallback callbackAsync) : this(callbackAsync, "[token]") { }


        public override Task AuthenticateAsync(WebRequestResult webRequestResult, string data) {
            return _callbackAsync.Invoke(this, webRequestResult, data);
        }
    }
}
