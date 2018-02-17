// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebServiceEventAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Rest.Services.Attributes.Events {
    [AttributeUsage(AttributeTargets.Event)]
    public class WebServiceEventAttribute : Attribute {
        public string Name { get; }

        public WebServiceEventAttribute(string name) {
            Name = name;
        }
    }
}
