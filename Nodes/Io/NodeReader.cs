using System;
using System.Collections.Generic;

namespace DotLogix.Core.Nodes.Io {
    public class NodeReader : NodeReaderBase {
        private readonly Node _node;

        public NodeReader(Node node) {
            _node = node;
        }

        protected override IEnumerable<NodeIoOp> EnumerateOps() {
            var list = new LinkedList<NodeIoOp>();
            AddOpsRecursive(_node, list);
            return list;
        }

        private static void AddOpsRecursive(Node node, ICollection<NodeIoOp> list) {
            switch(node.NodeType) {
                case NodeTypes.Empty:
                    list.Add(new NodeIoOp(NodeIoOpCodes.WriteValue, null, node.Name));
                    break;
                case NodeTypes.Value:
                    list.Add(new NodeIoOp(NodeIoOpCodes.WriteValue, null, node.Name));
                    break;
                case NodeTypes.List:
                    list.Add(new NodeIoOp(NodeIoOpCodes.BeginList, node.Name));
                    foreach(var children in ((NodeList)node).Children()) {
                        AddOpsRecursive(children, list);
                    }
                    list.Add(new NodeIoOp(NodeIoOpCodes.EndList));
                    break;
                case NodeTypes.Map:
                    list.Add(new NodeIoOp(NodeIoOpCodes.BeginMap, node.Name));
                    foreach(var children in ((NodeMap)node).Children()) {
                        AddOpsRecursive(children, list);
                    }
                    list.Add(new NodeIoOp(NodeIoOpCodes.EndMap));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}