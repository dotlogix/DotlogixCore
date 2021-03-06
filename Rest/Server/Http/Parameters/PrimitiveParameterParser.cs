// ==================================================
// Copyright 2019(C) , DotLogix
// File:  PrimitiveParameterParser.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  ..
// LastEdited:  12.06.2019
// ==================================================

using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace DotLogix.Core.Rest.Server.Http.Parameters {
    public class PrimitiveParameterParser : ParameterParserBase {
        public static IParameterParser Instance { get; } = new PrimitiveParameterParser();
        private PrimitiveParameterParser() { }

        public override void DeserializeValue(string name, string[] values, IDictionary<string, object> parameters) {
            if(values == null)
                return;

            switch(values.Length) {
                case 0:
                    parameters.Add(name, null);
                    break;
                case 1:
                    parameters.Add(name, values[0]);
                    break;
                default:
                    parameters.Add(name, values);
                    break;
            }
        }

        public override void SerializeValue(string name, object value, NameValueCollection collection) {
            if(!(value is Array array))
            {
                collection.Add(name, value.ToString());
                return;
            }

            foreach(var child in array)
                collection.Add(name, child.ToString());
        }
    }
}