// ==================================================
// Copyright 2018(C) , DotLogix
// File:  INodeWriter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.03.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Core.Nodes.Processor {
    public interface INodeWriter {
        void BeginMap();
        void BeginMap(string name);
        void EndMap();

        void BeginList();
        void BeginList(string name);
        void EndList();

        void WriteValue(string name, object value);
        void WriteValue(object value);

        void AutoComplete();

        void Execute(NodeOperation operation);
    }
}
