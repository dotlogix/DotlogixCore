// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebServiceRouteCollection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  05.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using DotLogix.Core.Rest.Server.Routes;
#endregion

namespace DotLogix.Core.Rest.Server.Http {
    public class WebServiceRouteCollection : List<IWebServiceRoute> { }
}
