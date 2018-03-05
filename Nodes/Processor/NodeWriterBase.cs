using System;
using System.Collections.Generic;

namespace DotLogix.Core.Nodes.Processor {
    public abstract class NodeWriterBase : INodeWriter {
        protected readonly Stack<NodeContainerType> ContainerStack = new Stack<NodeContainerType>();
        protected NodeContainerType CurrentContainer => ContainerStack.Count > 0 ? ContainerStack.Peek() : NodeContainerType.None;
        protected int ContainerCount => ContainerStack.Count;

        public virtual void BeginMap() => BeginMap(null);
        public abstract void BeginMap(string name);
        public abstract void EndMap();

        public virtual void BeginList() => BeginList(null);
        public abstract void BeginList(string name);
        public abstract void EndList();

        public virtual void WriteValue(object value) => WriteValue(null, value);
        public abstract void WriteValue(string name, object value);

        public virtual void AutoComplete() {
            while(ContainerStack.Count > 0) {
                switch(ContainerStack.Pop()) {
                    case NodeContainerType.Map:
                        EndMap();
                        break;
                    case NodeContainerType.List:
                        EndList();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void Execute(NodeOperation operation) {
            switch(operation.Type) {
                case NodeOperationTypes.BeginMap:
                    BeginMap(operation.Name);
                    break;
                case NodeOperationTypes.EndMap:
                    EndMap();
                    break;
                case NodeOperationTypes.BeginList:
                    BeginList(operation.Name);
                    break;
                case NodeOperationTypes.EndList:
                    EndList();
                    break;
                case NodeOperationTypes.WriteValue:
                    WriteValue(operation.Name, operation.Value);
                    break;
                case NodeOperationTypes.AutoComplete:
                    AutoComplete();
                    break;
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