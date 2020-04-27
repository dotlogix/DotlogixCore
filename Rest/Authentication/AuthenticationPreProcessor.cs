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
using DotLogix.Core.Rest.Http;
using DotLogix.Core.Rest.Services;
using DotLogix.Core.Rest.Services.Processors;
#endregion

namespace DotLogix.Core.Rest.Authentication {
    public class AuthenticationPreProcessor : WebRequestProcessorBase {
        private const string AuthorizationParameterName = "Authorization";
        private readonly IReadOnlyDictionary<string, IAuthenticationMethod> _authorizationMethods;

        public AuthenticationPreProcessor(int priority, IEnumerable<IAuthenticationMethod> authMethods) : base(priority) {
            _authorizationMethods = authMethods.ToDictionary(a => a.Name);
        }

        public AuthenticationPreProcessor(int priority, params IAuthenticationMethod[] authMethods) : this(priority, authMethods.AsEnumerable()) { }

        public override Task ProcessAsync(WebServiceContext context) {
            var authenticationDescriptor = context.Route.Descriptors.GetCustomDescriptor<AuthenticationDescriptor>();
            if((authenticationDescriptor != null) && (authenticationDescriptor.RequiresAuthentication == false))
                return Task.CompletedTask;

            var request = context.HttpRequest;
            var headerParameters = request.HeaderParameters;

            if(headerParameters.TryGetValueAs(AuthorizationParameterName, out string authParameter) == false) {
                SetInvalidFormatException(context);
                return Task.CompletedTask;
            }

            var authValue = authParameter.Split();
            if((authValue.Length != 2) || (_authorizationMethods.TryGetValue(authValue[0], out var authMethod) == false)) {
                SetInvalidFormatException(context);
                return Task.CompletedTask;
            }

            return authMethod.AuthenticateAsync(context, authValue[1]);
        }

        protected void SetUnauthorizedException(WebServiceContext context, string message) {
            var exception = new RestException(HttpStatusCodes.ClientError.Unauthorized, message);
            context.SetException(exception, HttpStatusCodes.ClientError.Unauthorized);
        }

        protected void SetInvalidFormatException(WebServiceContext context) {
            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine("The data you provided for authorization is in an invalid format.");
            messageBuilder.Append("Supported formats are:");

            foreach(var authorizationMethod in _authorizationMethods.Values) {
                foreach(var supportedDataFormat in authorizationMethod.SupportedDataFormats)
                    messageBuilder.Append($"\n\t{authorizationMethod.Name} {supportedDataFormat}");
            }

            SetUnauthorizedException(context, messageBuilder.ToString());
        }
    }
}
