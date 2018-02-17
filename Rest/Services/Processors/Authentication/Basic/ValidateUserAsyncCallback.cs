using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http;

namespace DotLogix.Core.Rest.Services.Processors.Authentication.Basic {
    public delegate Task ValidateUserAsyncCallback(BasicAuthenticationMethod authenticationMethod, WebRequestResult webRequestResult, string username, string password);
}