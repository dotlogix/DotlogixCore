// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Parameter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  29.01.2018
// LastEdited:  31.01.2018
// ==================================================

#region
using System.Collections.Generic;
using System.Linq;
#endregion

namespace DotLogix.Core.Rest.Server.Http.Parameters {
    public class Parameter {
        private readonly List<object> _values = new List<object>();
        public string Name { get; }
        public bool HasValues => _values.Count > 0;
        public bool IsMultiValue => _values.Count > 1;
        public object[] Values => _values.ToArray();
        public object Value => HasValues ? _values[0] : null;

        public Parameter(string name) {
            Name = name;
        }

        public Parameter(string name, IEnumerable<object> values) {
            Name = name;
            AddValues(values);
        }

        public Parameter(string name, params object[] values) : this(name, values.AsEnumerable()) { }

        public void AddValue(object value) {
            if(_values.Contains(value) == false)
                _values.Add(value);
        }

        public void AddValues(IEnumerable<object> values) {
            foreach(var value in values)
                AddValue(value);
        }

        public void Merge(Parameter parameter) {
            AddValues(parameter._values);
        }

        public object GetValue() {
            return HasValues ? _values[0] : null;
        }

        public override string ToString() {
            return $"{Name} = {string.Join(";", Values)}";
        }
    }
}
