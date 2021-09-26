using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DotLogix.Common.Features;
using DotLogix.WebServices.AspCore.Services;
using DotLogix.WebServices.Core.Terms;
using Microsoft.AspNetCore.Mvc;

namespace DotLogix.WebServices.AspCore.Controllers {
    public abstract class ReadOnlyWebService<TResponse> : WebServiceBase where TResponse : class, IGuid, new() {
        private IReadOnlyDomainService<TResponse> Service { get; }

        protected ReadOnlyWebService(IReadOnlyDomainService<TResponse> service) {
            Service = service;
        }
        
        [HttpGet("{guid:guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public virtual async Task<TResponse> GetAsync([FromRoute]Guid guid) {
            return await Service.GetAsync(guid);
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public virtual async Task<ICollection<TResponse>> GetAllAsync() {
            return await Service.GetAllAsync();
        }
    }

    public class ReadOnlyWebService<TResponse, TFilter> : ReadOnlyWebService<TResponse> where TResponse : class, IGuid, new() {
        private IReadOnlyDomainService<TResponse, TFilter> Service { get; }

        public ReadOnlyWebService(IReadOnlyDomainService<TResponse, TFilter> service) : base(service) {
            Service = service;
        }
        
        [HttpGet("filtered")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public virtual async Task<ICollection<TResponse>> GetFilteredAsync([FromBody]TFilter filter) {
            return await Service.GetFilteredAsync(filter);
        }
    }
}
