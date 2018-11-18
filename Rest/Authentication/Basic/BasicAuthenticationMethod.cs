// ==================================================
// Copyright 2018(C) , DotLogix
// File:  BasicAuthenticationMethod.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using DotLogix.Core.Rest.Authentication.Base;
using DotLogix.Core.Rest.Server.Http;
#endregion

namespace DotLogix.Core.Rest.Authentication.Basic {
    public class BasicAuthenticationMethod : AuthenticationMethodBase {
        private readonly ValidateUserAsyncCallback _callbackAsync;


        public BasicAuthenticationMethod(ValidateUserAsyncCallback callbackAsync) : base("Basic", "[username]:[password] encoded in bas64") {
            _callbackAsync = callbackAsync;
        }

        public override Task AuthenticateAsync(WebRequestResult webRequestResult, string data) {
            var dataSplit = StringExtensions.DecodeBase64(data).Split(':');
            if(dataSplit.Length == 2)
                return _callbackAsync.Invoke(this, webRequestResult, dataSplit[0], dataSplit[1]);

            SetInvalidFormatException(webRequestResult);
            return Task.CompletedTask;
        }
    }
}
