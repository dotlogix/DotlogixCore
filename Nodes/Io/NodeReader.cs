// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeReader.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Nodes.Io {
    public class NodeReader : NodeReaderBase {
        private readonly Node _node;

        public NodeReader(Node node) {
            _node = node;
        }

        public override void CopyTo(INodeWriter nodeWriter) {
            AddOpsRecursive(_node, nodeWriter);
        }

        private static void AddOpsRecursive(Node node, INodeWriter nodeWriter) {
            switch(node.NodeType) {
                case NodeTypes.Empty:
                    nodeWriter.WriteValue(null, node.Name);
                    break;
                case NodeTypes.Value:
                    nodeWriter.WriteValue(null, node.Name);
                    break;
                case NodeTypes.List:
                    nodeWriter.BeginList(node.Name);
                    foreach(var children in ((NodeList)node).Children())
                        AddOpsRecursive(children, nodeWriter);
                    nodeWriter.EndList();
                    break;
                case NodeTypes.Map:
                    nodeWriter.BeginMap(node.Name);
                    foreach(var children in ((NodeMap)node).Children())
                        AddOpsRecursive(children, nodeWriter);
                    nodeWriter.EndMap();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
