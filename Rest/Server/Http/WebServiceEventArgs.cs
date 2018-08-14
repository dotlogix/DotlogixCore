// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebServiceEventArgs.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  05.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Rest.Server.Http {
    public class WebServiceEventArgs : EventArgs {
        public string Channel { get; }

        public WebServiceEventArgs(string channel = null) {
            Channel = channel;
        }
    }
}
