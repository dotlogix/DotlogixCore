// ==================================================
// Copyright 2018(C) , DotLogix
// File:  AuthenticationPreProcessor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using DotLogix.Core.Rest.Authentication.Base;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Server.Http.State;
using DotLogix.Core.Rest.Services.Context;
using DotLogix.Core.Rest.Services.Exceptions;
#endregion

namespace DotLogix.Core.Rest.Services.Processors.Authentication {
    public class AuthenticationPreProcessor : WebRequestProcessorBase {
        private const string AuthorizationParameterName = "Authorization";
        private readonly IReadOnlyDictionary<string, IAuthenticationMethod> _authorizationMethods;

        public AuthenticationPreProcessor(int priority, IEnumerable<IAuthenticationMethod> authMethods) : base(priority) {
            _authorizationMethods = authMethods.ToDictionary(a => a.Name);
        }

        public AuthenticationPreProcessor(int priority, params IAuthenticationMethod[] authMethods) : this(priority, authMethods.AsEnumerable()) { }

        public override Task ProcessAsync(WebServiceContext webServiceContext) {
            var authenticationDescriptor = webServiceContext.Route.RequestProcessor.Descriptors.GetCustomDescriptor<AuthenticationDescriptor>();
            if((authenticationDescriptor != null) && (authenticationDescriptor.RequiresAuthentication == false))
                return Task.CompletedTask;

            var request = webServiceContext.HttpRequest;
            var result = webServiceContext.RequestResult;
            var headerParameters = request.HeaderParameters;

            if(headerParameters.TryGetValue(AuthorizationParameterName, out string authParameter) == false) {
                SetInvalidFormatException(result);
                return Task.CompletedTask;
            }

            var authValue = authParameter.Split();
            if((authValue.Length != 2) || (_authorizationMethods.TryGetValue(authValue[0], out var authMethod) == false)) {
                SetInvalidFormatException(result);
                return Task.CompletedTask;
            }

            return authMethod.AuthenticateAsync(result, authValue[1]);
        }

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
