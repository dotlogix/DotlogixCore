using System;

namespace DotLogix.Core.Nodes.Io {
    public abstract class NodeWriterBase : INodeWriter {
        private string _currentName;

        protected NodeIoStateMachine StateMachine { get; } =
            new NodeIoStateMachine(NodeIoState.None, NodeIoOpCodes.BeginAny);

        public abstract void BeginMap(string name = null);

        public abstract void EndMap();

        public abstract void BeginList(string name = null);

        public abstract void EndList();

        public abstract void WriteValue(object value, string name = null);

        public virtual void AutoComplete() {
            while(StateMachine.CurrentState != NodeIoState.None) {
                switch(StateMachine.CurrentState) {
                    case NodeIoState.InsideMap:
                        EndMap();
                        break;
                    case NodeIoState.InsideList:
                        EndList();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public virtual void ExecuteOperation(NodeIoOp operation) {
            switch(operation.OpCode) {
                case NodeIoOpCodes.None:
                    break;
                case NodeIoOpCodes.BeginMap:
                    BeginMap(operation.GetArg<string>(0) ?? _currentName);
                    break;
                case NodeIoOpCodes.EndMap:
                    EndMap();
                    break;
                case NodeIoOpCodes.BeginList:
                    BeginList(operation.GetArg<string>(0) ?? _currentName);
                    break;
                case NodeIoOpCodes.EndList:
                    EndList();
                    break;
                case NodeIoOpCodes.WriteValue:
                    WriteValue(operation.GetArg(0), operation.GetArg<string>(1) ?? _currentName);
                    break;
                case NodeIoOpCodes.SetName:
                    _currentName = operation.GetArg<string>(0);
                    return;
                case NodeIoOpCodes.AutoComplete:
                    AutoComplete();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _currentName = null;
        }
    }
}