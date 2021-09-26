using DotLogix.Core.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DotLogix.WebServices.AspCore.Controllers {
    public class WebServiceBase : ControllerBase {
        private ILogSource _logger;
        protected ILogSource Logger => _logger ??= ServiceProviders.LogSourceProvider.Create(GetType().FullName);
    }
}