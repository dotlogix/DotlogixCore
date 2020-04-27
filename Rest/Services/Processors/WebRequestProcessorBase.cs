// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebRequestProcessorBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.06.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Threading.Tasks;
using DotLogix.Core.Rest.Services.Descriptors;
#endregion

namespace DotLogix.Core.Rest.Services.Processors {
    public abstract class WebRequestProcessorBase : IWebRequestProcessor {
        public bool IgnoreHandled { get; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        protected WebRequestProcessorBase(int priority, bool ignoreHandled = true) {
            Priority = priority;
            IgnoreHandled = ignoreHandled;
        }

        public int Priority { get; }

        public abstract Task ProcessAsync(WebServiceContext context);

        public virtual bool ShouldExecute(WebServiceContext webServiceContext) {
            return webServiceContext.Result == null || (IgnoreHandled == false);
        }
    }
}
