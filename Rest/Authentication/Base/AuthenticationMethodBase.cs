// ==================================================
// Copyright 2018(C) , DotLogix
// File:  AuthenticationMethodBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Text;
using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Server.Http.State;
using DotLogix.Core.Rest.Services.Context;
using DotLogix.Core.Rest.Services.Exceptions;
#endregion

namespace DotLogix.Core.Rest.Authentication.Base {
    public abstract class AuthenticationMethodBase : IAuthenticationMethod {
        protected AuthenticationMethodBase(string name, params string[] supportedDataFormats) {
            Name = name;
            SupportedDataFormats = supportedDataFormats;
        }

        public string Name { get; }
        public string[] SupportedDataFormats { get; }
        public abstract Task AuthenticateAsync(WebServiceContext context, string data);

        public void SetUnauthorizedException(WebServiceContext context, string message) {
            context.RequestResult.SetException(new RestException(HttpStatusCodes.ClientError.Unauthorized, message));
        }

        public void SetInvalidFormatException(WebServiceContext context) {
            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine("The data you provided for authorization is in an invalid format.");
            messageBuilder.Append(SupportedDataFormats.Length != 1 ? "Supported formats are:" : "The supported format is:");

            foreach(var supportedDataFormat in SupportedDataFormats)
                messageBuilder.Append($"\n\t{Name} {supportedDataFormat}");

            SetUnauthorizedException(context, messageBuilder.ToString());
        }
    }
}
