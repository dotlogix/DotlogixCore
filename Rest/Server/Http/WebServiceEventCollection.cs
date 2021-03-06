// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebServiceEventCollection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  05.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Core.Collections;
#endregion

namespace DotLogix.Core.Rest.Server.Http {
    public class WebServiceEventCollection : KeyedCollection<string, IWebServiceEvent> {
        public WebServiceEventCollection() : base(e => e.Name) { }
    }
}
