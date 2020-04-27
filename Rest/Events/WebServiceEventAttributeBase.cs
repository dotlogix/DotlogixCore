// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebServiceEventAttributeBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  05.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Rest.Events {
    [AttributeUsage(AttributeTargets.Event)]
    public abstract class WebServiceEventAttributeBase : Attribute {
        public string Name { get; }

        protected WebServiceEventAttributeBase(string name) {
            Name = name;
        }

        public abstract IWebServiceEvent CreateEvent();
    }
}
