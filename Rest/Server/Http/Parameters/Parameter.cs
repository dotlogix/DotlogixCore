// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Parameter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Rest.Server.Http.Parameters {
    public struct Parameter {
        public string Name { get; }
        public IEnumerable<object> Values { get; }

        public Parameter(string name, IEnumerable<object> values) {
            Name = name;
            Values = values;
        }
    }
}
