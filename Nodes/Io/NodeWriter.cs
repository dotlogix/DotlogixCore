using System;
using DotLogix.Core.Extensions;
using DotLogix.Core.Types;

namespace DotLogix.Core.Nodes.Io
{
    public class NodeWriter : NodeWriterBase {
        private readonly bool _checkDataTypes;
        private NodeCollection _ancestor;
        private Node _root;
        public NodeWriter(bool checkDataTypes = true) {
            _checkDataTypes = checkDataTypes;
        }

        public Node Root {
            get {
                if (StateMachine.CurrentState != NodeIoState.None && _root != null)
                    throw new InvalidOperationException("NodeWriter has not completed at the moment");

                return _root;
            }
        }

        public override void BeginMap(string name = null) {
            if(StateMachine.GoToState(NodeIoState.InsideMap, NodeIoOpCodes.BeginMap, NodeIoOpCodes.BeginAny | NodeIoOpCodes.EndMap) == false)
                throw new InvalidOperationException($"{NodeIoOpCodes.BeginMap} operation in state {StateMachine.CurrentState} is not allowed");
            
            var node = new NodeMap();
            GoToNewChild(name, node);
        }

        public override void EndMap() {
            GoToParent(NodeIoOpCodes.EndMap);
        }

        public override void BeginList(string name = null)
        {
            if (StateMachine.GoToState(NodeIoState.InsideList, NodeIoOpCodes.BeginList, NodeIoOpCodes.BeginAny | NodeIoOpCodes.EndList) == false)
                throw new InvalidOperationException($"{NodeIoOpCodes.BeginList} operation in state {StateMachine.CurrentState} is not allowed");

            var node = new NodeList();
            GoToNewChild(name, node);
        }

        public override void EndList()
        {
            GoToParent(NodeIoOpCodes.EndList);
        }

        public override void WriteValue(object value, string name = null) {
            if(StateMachine.IsAllowedOperation(NodeIoOpCodes.WriteValue) == false)
                throw new InvalidOperationException($"{NodeIoOpCodes.WriteValue} operation in state {StateMachine.CurrentState} is not allowed");

            DataType dataType=null;
            if (_checkDataTypes) {
                dataType = value.GetDataType();
                if (value != null && (dataType.Flags & DataTypeFlags.Primitive) == 0)
                    throw new InvalidOperationException("Value has to be a pimitive"); 
            }

            var node = new NodeValue(name, value, dataType);
            switch (StateMachine.CurrentState)
            {
                case NodeIoState.None:
                    if (name != null)
                        throw new InvalidOperationException("Constuctor nodes can not have a name");
                    _root = new NodeValue();
                    StateMachine.GoToState(NodeIoState.None, NodeIoOpCodes.WriteValue, NodeIoOpCodes.None);
                    break;
                case NodeIoState.InsideMap:
                    if (name == null)
                        throw new ArgumentNullException(nameof(name), "You need a name to add this node to a node map");
                    _ancestor.AddChild(node);
                    break;
                case NodeIoState.InsideList:
                    if (name != null)
                        throw new InvalidOperationException("Children in a node list can not have a name");
                    _ancestor.AddChild(node);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private void GoToParent(NodeIoOpCodes withOperation) {
            NodeIoOperation operation;
            operation.OpCode = withOperation;

            var ancestor = _ancestor.Ancestor;
            if (ancestor == null)
            {
                operation.NextState = NodeIoState.None;
                operation.AllowedNextOpCodes = NodeIoOpCodes.None;
            }
            else
            {
                switch (ancestor.NodeType)
                {
                    case NodeTypes.List:
                        operation.NextState = NodeIoState.InsideList;
                        operation.AllowedNextOpCodes = NodeIoOpCodes.BeginAny | NodeIoOpCodes.EndList;
                        break;
                    case NodeTypes.Map:
                        operation.NextState = NodeIoState.InsideMap;
                        operation.AllowedNextOpCodes = NodeIoOpCodes.BeginAny | NodeIoOpCodes.EndMap;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (StateMachine.GoToState(operation) == false)
                throw new InvalidOperationException($"{operation.OpCode} operation in state {StateMachine.CurrentState} is not allowed");

            _ancestor = ancestor;
        }

        private void GoToNewChild(string name, NodeCollection node) {
            switch(StateMachine.PreviousState) {
                case NodeIoState.None:
                    if (name != null)
                        throw new InvalidOperationException("Constuctor nodes can not have a name");
                    _root = node;
                    break;
                case NodeIoState.InsideMap:
                    if(name == null)
                        throw new ArgumentNullException(nameof(name), "You need a name to add this node to a node map");
                    node.InternalName = name;
                    _ancestor.AddChild(node);
                    break;
                case NodeIoState.InsideList:
                    if (name != null)
                        throw new InvalidOperationException("Children in a node list can not have a name");
                    _ancestor.AddChild(node);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _ancestor = node;
        }
    }
}
