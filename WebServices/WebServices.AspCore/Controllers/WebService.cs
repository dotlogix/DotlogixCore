using System;
using System.Net;
using System.Threading.Tasks;
using DotLogix.Common.Features;
using DotLogix.WebServices.AspCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotLogix.WebServices.AspCore.Controllers {
    public abstract class WebService<TResponse, TCreate, TEnsure, TPatch> : ReadOnlyWebService<TResponse>
        where TResponse : class, IGuid, new()
        where TCreate : class, IGuid
        where TEnsure : class, IGuid
        where TPatch : class, IGuid
    {
        private IDomainService<TResponse, TCreate, TEnsure, TPatch> Service { get; }
        
        protected WebService(IDomainService<TResponse, TCreate, TEnsure, TPatch> service) : base(service) {
            Service = service;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public virtual async Task<ActionResult<TResponse>> CreateAsync([FromBody] TCreate request) {
            var response = await Service.CreateAsync(request);
            // ReSharper disable once Mvc.ActionNotResolved
            return CreatedAtAction(Url.Action(nameof(GetAsync), new { guid = response.Guid }), response);
        }

        [HttpPut("{guid:guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public virtual async Task<ActionResult<TResponse>> PutAsync([FromRoute]Guid guid, [FromBody] TEnsure request) {
            request.Guid = guid;
            var response = await Service.EnsureAsync(request);
            return Ok(response);
        }

        [HttpPatch("{guid:guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public virtual async Task<ActionResult<TResponse>> PatchAsync([FromRoute]Guid guid, [FromBody] TPatch request) {
            request.Guid = guid;
            var response = await Service.PatchAsync(request);

            return response != null ? (ActionResult<TResponse>)Ok(response) : NotFound();
        }

        [HttpDelete("{guid:guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public virtual async Task<IActionResult> DeleteAsync([FromRoute]Guid guid) {
            
            var removed = await Service.RemoveAsync(guid);
            return removed ? (IActionResult)Ok() : NotFound();
        }
    }
    
    
    
    public abstract class WebService<TResponse, TEnsure> : WebService<TResponse, TEnsure, TEnsure, TEnsure>
        where TResponse : class, IGuid, new()
        where TEnsure : class, IGuid {
        
        protected WebService(IDomainService<TResponse, TEnsure, TEnsure, TEnsure> service) : base(service) { }
    }
}
