// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebRequestProcessorCollection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  05.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Core.Collections;
using DotLogix.Core.Rest.Services.Processors;
#endregion

namespace DotLogix.Core.Rest.Server.Http {
    public class WebRequestProcessorCollection : SortedCollection<IWebRequestProcessor> {
        public WebRequestProcessorCollection() : base(WebRequestProcessorComparer.Instance) { }
    }
}
