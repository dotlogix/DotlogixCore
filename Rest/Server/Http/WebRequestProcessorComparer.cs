// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebRequestProcessorComparer.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using DotLogix.Core.Rest.Services.Processors;
#endregion

namespace DotLogix.Core.Rest.Server.Http {
    public class WebRequestProcessorComparer : IComparer<IWebRequestProcessor> {
        public static IComparer<IWebRequestProcessor> Instance { get; } = new WebRequestProcessorComparer();

        private WebRequestProcessorComparer() { }


        public int Compare(IWebRequestProcessor x, IWebRequestProcessor y) {
            if(x == null)
                throw new ArgumentNullException(nameof(x));
            if(y == null)
                throw new ArgumentNullException(nameof(y));
            return y.Priority.CompareTo(x.Priority);
        }
    }
}
