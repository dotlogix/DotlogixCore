// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeReader.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Nodes.Processor {
    public class NodeReader : NodeReaderBase {
        public Node Node { get; }

        public NodeReader(Node node) {
            Node = node;
        }

        public override void CopyTo(INodeWriter writer) {
            CopyToRecursive(Node, writer);
        }

        protected void CopyToRecursive(Node node, INodeWriter writer) {
            switch(node.Type) {
                case NodeTypes.Empty:
                    writer.WriteValue(node.Name, null);
                    break;
                case NodeTypes.Value:
                    writer.WriteValue(node.Name, ((NodeValue)node).Value);
                    break;
                case NodeTypes.List:
                    writer.BeginList(node.Name);
                    foreach(var child in ((NodeList)node).Children())
                        CopyToRecursive(child, writer);
                    writer.EndList();
                    break;
                case NodeTypes.Map:
                    writer.BeginMap(node.Name);
                    foreach(var child in ((NodeMap)node).Children())
                        CopyToRecursive(child, writer);
                    writer.EndMap();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
