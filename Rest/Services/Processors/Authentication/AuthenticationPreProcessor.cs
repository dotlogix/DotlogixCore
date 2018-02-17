// ==================================================
// Copyright 2018(C) , DotLogix
// File:  AuthenticationPreProcessor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Server.Http.State;
using DotLogix.Core.Rest.Services.Exceptions;
using DotLogix.Core.Rest.Services.Processors.Authentication.Base;
#endregion

namespace DotLogix.Core.Rest.Services.Processors.Authentication {
    public class AuthenticationPreProcessor : IWebRequestProcessor {
        private const string AuthorizationParameterName = "Authorization";
        private readonly IReadOnlyDictionary<string, IAuthenticationMethod> _authorizationMethods;

        public AuthenticationPreProcessor(int priority, IEnumerable<IAuthenticationMethod> authMethods) {
            Priority = priority;
            _authorizationMethods = authMethods.ToDictionary(a => a.Name);
        }

        public AuthenticationPreProcessor(int priority, params IAuthenticationMethod[] authMethods) : this(priority, authMethods.AsEnumerable()) { }

        public Task ProcessAsync(WebRequestResult webRequestResult) {
            var request = webRequestResult.Context.Request;
            var headerParameters = request.HeaderParameters;

            if((headerParameters.TryGetParameter(AuthorizationParameterName, out var authParameter) == false) || (authParameter.HasValues == false)) {
                SetInvalidFormatException(webRequestResult);
                return Task.CompletedTask;
            }

            var authValue = ((string)authParameter.Value).Split();
            if((authValue.Length != 2) || (_authorizationMethods.TryGetValue(authValue[0], out var authMethod) == false)) {
                SetInvalidFormatException(webRequestResult);
                return Task.CompletedTask;
            }


            return authMethod.AuthenticateAsync(webRequestResult, authValue[1]);
        }

        public int Priority { get; }
        public bool IgnoreHandled => false;

        protected void SetUnauthorizedException(WebRequestResult webRequestResult, string message) {
            webRequestResult.SetException(new RestException(HttpStatusCodes.ClientError.Unauthorized, message));
        }

        protected void SetInvalidFormatException(WebRequestResult webRequestResult) {
            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine("The data you provided for authorization is in an invalid format.");
            messageBuilder.Append("Supported formats are:");

            foreach(var authorizationMethod in _authorizationMethods.Values) {
                foreach(var supportedDataFormat in authorizationMethod.SupportedDataFormats)
                    messageBuilder.Append($"\n\t{authorizationMethod.Name} {supportedDataFormat}");
            }

            SetUnauthorizedException(webRequestResult, messageBuilder.ToString());
        }
    }
}
