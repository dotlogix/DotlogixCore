// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebServiceEventAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Core.Rest.Server.Http;
#endregion

namespace DotLogix.Core.Rest.Services.Attributes.Events {
    public class WebServiceEventAttribute : WebServiceEventAttributeBase {
        public WebServiceEventAttribute(string name) : base(name) { }

        public override IWebServiceEvent CreateEvent() {
            return new WebServiceEvent(Name);
        }
    }
}
