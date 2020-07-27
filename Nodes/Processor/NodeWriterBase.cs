// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeWriterBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Threading.Tasks;
#endregion

namespace DotLogix.Core.Nodes.Processor {
    public abstract class NodeWriterBase : IAsyncNodeWriter {
        protected readonly NodeContainerStack ContainerStack = new NodeContainerStack();
        protected NodeWriterBase(ConverterSettings converterSettings = null) {  
            ConverterSettings = converterSettings ?? ConverterSettings.Default;
        }
        protected ConverterSettings ConverterSettings { get; }

        public virtual ValueTask BeginMapAsync() => BeginMapAsync(null);
        public abstract ValueTask BeginMapAsync(string name);
        public abstract ValueTask EndMapAsync();

        public virtual ValueTask BeginListAsync() => BeginListAsync(null);
        public abstract ValueTask BeginListAsync(string name);
        public abstract ValueTask EndListAsync();

        public virtual ValueTask WriteValueAsync(object value) => WriteValueAsync(null, value);
        public abstract ValueTask WriteValueAsync(string name, object value);

        public virtual async ValueTask AutoCompleteAsync() {
            while(ContainerStack.Count > 0) {
                switch(ContainerStack.Pop()) {
                    case NodeContainerType.Map:
                        await EndMapAsync().ConfigureAwait(false);
                        break;
                    case NodeContainerType.List:
                        await EndListAsync().ConfigureAwait(false);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public ValueTask ExecuteAsync(NodeOperation operation) {
            switch(operation.Type) {
                case NodeOperationTypes.BeginMap:
                    return BeginMapAsync(operation.Name);
                case NodeOperationTypes.EndMap:
                    return EndMapAsync();
                case NodeOperationTypes.BeginList:
                    return BeginListAsync(operation.Name);
                case NodeOperationTypes.EndList:
                    return EndListAsync();
                case NodeOperationTypes.Value:
                    return WriteValueAsync(operation.Name, operation.Value);
                case NodeOperationTypes.AutoComplete:
                    return AutoCompleteAsync();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
