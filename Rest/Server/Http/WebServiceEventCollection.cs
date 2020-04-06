// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebServiceEventCollection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  05.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using DotLogix.Core.Collections;
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Services.Processors.Json;
#endregion

namespace DotLogix.Core.Rest.Server.Http {
    public class WebServiceEventCollection : KeyedCollection<string, IWebServiceEvent> {
        /// <inheritdoc />
        public WebServiceEventCollection() : base(e => e.Name) { }
    }

    public class ParameterProviderCollection : List<IParameterProvider> {
        /// <inheritdoc />
        public ParameterProviderCollection() { }

        /// <inheritdoc />
        public ParameterProviderCollection(IEnumerable<IParameterProvider> collection) : base(collection) { }

        /// <inheritdoc />
        public ParameterProviderCollection(int capacity) : base(capacity) { }

        public void InsertBefore(string name, IParameterProvider provider) {
            var index = FindIndex(p => p.Name == name);
            Insert(index, provider);
        }

        public void InsertAfter(string name, IParameterProvider provider) {
            var index = FindIndex(p => p.Name == name);
            Insert(index + 1, provider);
        }

        public void InsertBefore(ParameterSources source, IParameterProvider provider) {
            var index = FindIndex(p => p.Source == source);
            Insert(index, provider);
        }

        public void InsertAfter(ParameterSources source, IParameterProvider provider) {
            var index = FindLastIndex(p => p.Source == source);
            Insert(index + 1, provider);
        }
    }
}
