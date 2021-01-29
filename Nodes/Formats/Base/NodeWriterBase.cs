// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeWriterBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Nodes.Formats.Nodes;
using DotLogix.Core.Nodes.Schema;
#endregion

namespace DotLogix.Core.Nodes.Formats {
    public abstract class NodeWriterBase : INodeWriter {
        protected readonly NodeContainerStack ContainerStack = new NodeContainerStack();
        protected NodeWriterBase(IReadOnlyConverterSettings converterSettings = null) {  
            ConverterSettings = converterSettings ?? Schema.ConverterSettings.Default;
        }

        protected string CurrentName { get; set; }
        protected IReadOnlyConverterSettings ConverterSettings { get; set; }

        #region 

        public void WriteName(string name)
        {
            CurrentName = name;
        }

        public abstract void WriteBeginMap();
        public abstract void WriteEndMap();

        public abstract void WriteBeginList();
        public abstract void WriteEndList();

        public abstract void WriteValue(object value);

        public void WriteOperation(NodeOperation operation)
        {
            var name = operation.Name ?? CurrentName;
            CurrentName = null;
            if (string.IsNullOrEmpty(name) == false)
                WriteName(name);

            switch(operation.Type) {
                case NodeOperationTypes.BeginMap:
                    WriteBeginMap();
                    break;
                case NodeOperationTypes.EndMap:
                    WriteEndMap();
                    break;
                case NodeOperationTypes.BeginList:
                    WriteBeginList();
                    break;
                case NodeOperationTypes.EndList:
                    WriteEndList();
                    break;
                case NodeOperationTypes.Value:
                    WriteValue(operation.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
