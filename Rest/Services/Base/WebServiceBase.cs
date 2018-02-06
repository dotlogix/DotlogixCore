// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebServiceBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  29.01.2018
// LastEdited:  31.01.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Rest.Services.Base {
    public abstract class WebServiceBase : IWebService {
        protected WebServiceBase(string routePrefix, string name = null) {
            Name = name ?? GetType().Name;
            RoutePrefix = routePrefix ?? throw new ArgumentNullException(nameof(routePrefix));
        }

        public string Name { get; }
        public string RoutePrefix { get; }
    }
}
