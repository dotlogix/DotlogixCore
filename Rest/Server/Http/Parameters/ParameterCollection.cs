// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ParameterCollection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  07.02.2018
// ==================================================

#region
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
#endregion

namespace DotLogix.Core.Rest.Server.Http.Parameters {
    public class ParameterCollection : IEnumerable<Parameter> {
        private readonly Dictionary<string, Parameter> _parametersDict = new Dictionary<string, Parameter>(StringComparer.OrdinalIgnoreCase);
        public Parameter this[string name] => GetParameter(name);
        public bool HasValues => _parametersDict.Count > 0;

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public IEnumerator<Parameter> GetEnumerator() {
            return _parametersDict.Values.GetEnumerator();
        }

        public void Add(Parameter parameter, bool mergeIfExists = true) {
            if(parameter == null)
                return;
            if(_parametersDict.TryGetValue(parameter.Name, out var existing)) {
                if(mergeIfExists)
                    existing.Merge(parameter);
                else
                    throw new InvalidOperationException($"Parameter {existing.Name} does already exist");
            } else
                _parametersDict.Add(parameter.Name, parameter);
        }

        public void Set(Parameter parameter, bool mergeIfExists = true) {
            if(parameter == null)
                return;
            if(mergeIfExists && _parametersDict.TryGetValue(parameter.Name, out var existing)) {
                existing.Merge(parameter);
                return;
            }
            _parametersDict[parameter.Name] = parameter;
        }

        public void AddRange(IEnumerable<Parameter> parameters, bool mergeIfExists = true) {
            if(parameters == null)
                return;
            foreach(var parameter in parameters)
                Add(parameter, mergeIfExists);
        }

        public void AppendString(StringBuilder builder) {
            foreach(var httpParameter in _parametersDict.Values)
                builder.AppendLine(httpParameter.ToString());
        }

        public override string ToString() {
            var builder = new StringBuilder();
            AppendString(builder);
            return builder.ToString();
        }

        #region Parameter
        public Parameter GetParameter(string name) {
            return _parametersDict.TryGetValue(name, out var existing) ? existing : null;
        }

        public object GetParameterValue(string name) {
            return GetParameter(name)?.Value;
        }

        public object[] GetParameterValues(string name) {
            return GetParameter(name)?.Values;
        }
        #endregion

        #region TryParameter
        public bool TryGetParameter(string name, out Parameter parameter) {
            return _parametersDict.TryGetValue(name, out parameter);
        }

        public bool TryGetParameterValue(string name, out object value) {
            if(_parametersDict.TryGetValue(name, out var parameter) == false) {
                value = default(object);
                return false;
            }
            value = parameter.GetValue();
            return true;
        }
        #endregion
    }
}
