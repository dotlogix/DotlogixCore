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
using DotLogix.Core.Rest.Http;
using DotLogix.Core.Rest.Services;
#endregion

namespace DotLogix.Core.Rest.Authentication {
    public abstract class AuthenticationMethodBase : IAuthenticationMethod {
        protected AuthenticationMethodBase(string name, params string[] supportedDataFormats) {
            Name = name;
            SupportedDataFormats = supportedDataFormats;
        }

        public string Name { get; }
        public string[] SupportedDataFormats { get; }
        public abstract Task AuthenticateAsync(WebServiceContext context, string data);

        public void SetUnauthorizedException(WebServiceContext context, string message) {
            var exception = new RestException(HttpStatusCodes.ClientError.Unauthorized, message);
            context.SetException(exception, HttpStatusCodes.ClientError.Unauthorized);
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
