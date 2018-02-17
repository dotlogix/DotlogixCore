// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebRequestProcessorComparer.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Collections.Generic;
using DotLogix.Core.Rest.Services.Processors;
#endregion

namespace DotLogix.Core.Rest.Server.Http {
    public class WebRequestProcessorComparer : IComparer<IWebRequestProcessor> {
        public static IComparer<IWebRequestProcessor> Instance { get; } = new WebRequestProcessorComparer();

        private WebRequestProcessorComparer() { }


        public int Compare(IWebRequestProcessor x, IWebRequestProcessor y) {
            return y.Priority.CompareTo(x.Priority);
        }
    }
}
