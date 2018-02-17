// ==================================================
// Copyright 2018(C) , DotLogix
// File:  INodeWriter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

namespace DotLogix.Core.Nodes.Io {
    public interface INodeWriter {
        void BeginMap(string name = null);
        void EndMap();

        void BeginList(string name = null);
        void EndList();

        void WriteValue(object value, string name = null);
        void AutoComplete();
        void ExecuteOperation(NodeIoOp operation);
    }
}
