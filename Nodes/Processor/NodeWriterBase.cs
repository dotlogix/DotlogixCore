// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeWriterBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#endregion

namespace DotLogix.Core.Nodes.Processor {
    public abstract class NodeWriterBase : IAsyncNodeWriter {
        protected readonly Stack<NodeContainerType> ContainerStack = new Stack<NodeContainerType>();
        protected NodeWriterBase(ConverterSettings converterSettings = null) {
            ConverterSettings = converterSettings ?? ConverterSettings.Default;
        }
        protected NodeContainerType CurrentContainer => ContainerStack.Count > 0 ? ContainerStack.Peek() : NodeContainerType.None;
        protected int ContainerCount => ContainerStack.Count;
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
                ValueTask task;
                switch(ContainerStack.Pop()) {
                    case NodeContainerType.Map:
                        task = EndMapAsync();
                        break;
                    case NodeContainerType.List:
                        task = EndListAsync();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if(task.IsCompletedSuccessfully == false)
                    await task;
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
                case NodeOperationTypes.WriteValue:
                    return WriteValueAsync(operation.Name, operation.Value);
                case NodeOperationTypes.AutoComplete:
                    return AutoCompleteAsync();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected void PushContainer(NodeContainerType containerType) {
            ContainerStack.Push(containerType);
        }

        protected void PopExpectedContainer(NodeContainerType expectedType) {
            if(ContainerStack.Count == 0)
                throw new InvalidOperationException("There is nothing on the container stack");

            var currentContainer = ContainerStack.Pop();
            if(currentContainer == expectedType)
                return;

            ContainerStack.Push(currentContainer);
            throw new InvalidOperationException($"The current container doesn't match the expected type {expectedType} (current: {currentContainer})");
        }

        protected NodeContainerType PopContainer() {
            return ContainerStack.Pop();
        }
    }
}
