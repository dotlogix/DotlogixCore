// ==================================================
// Copyright 2018(C) , DotLogix
// File:  INodeWriter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.03.2018
// LastEdited:  01.08.2018
// ==================================================

using System.Threading.Tasks;

namespace DotLogix.Core.Nodes.Processor {
    public interface IAsyncNodeWriter {
        ValueTask BeginMapAsync();
        ValueTask BeginMapAsync(string name);
        ValueTask EndMapAsync();

        ValueTask BeginListAsync();
        ValueTask BeginListAsync(string name);
        ValueTask EndListAsync();

        ValueTask WriteValueAsync(string name, object value);
        ValueTask WriteValueAsync(object value);

        ValueTask AutoCompleteAsync();

        ValueTask ExecuteAsync(NodeOperation operation);
    }
}
