// ==================================================
// Copyright 2019(C) , DotLogix
// File:  ParameterParserBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  ..
// LastEdited:  12.06.2019
// ==================================================

using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace DotLogix.Core.Rest.Server.Http.Parameters {
    public abstract class ParameterParserBase : IParameterParser {
        public IDictionary<string, object> Deserialize(NameValueCollection collection) {
            var parameters = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            for(var i = 0; i < collection.Count; i++) {
                var name = collection.GetKey(i);
                var values = collection.GetValues(i);

                DeserializeValue(name, values, parameters);
            }
            return parameters;
        }

        public TCollection Serialize<TCollection>(IDictionary<string, object> parameters) where TCollection : NameValueCollection, new() {
            var collection = new TCollection();
            foreach(var parameter in parameters) {
                SerializeValue(parameter.Key, parameter.Value, collection);
            }
            return collection;
        }

        public abstract void DeserializeValue(string name, string[] values, IDictionary<string, object> parameters);
        public abstract void SerializeValue(string name, object value, NameValueCollection collection);
    }
}