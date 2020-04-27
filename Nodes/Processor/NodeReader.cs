// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeReader.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Threading.Tasks;
#endregion

namespace DotLogix.Core.Nodes.Processor {
    public class NodeReader : NodeReaderBase {
        public Node Node { get; }

        public NodeReader(Node node) {
            Node = node;
        }

        public override ValueTask CopyToAsync(IAsyncNodeWriter writer) {
            return CopyToRecursiveAsync(Node, writer);
        }

        protected async ValueTask CopyToRecursiveAsync(Node node, IAsyncNodeWriter writer) {
            switch(node.Type) {
                case NodeTypes.Empty:
                    await writer.WriteValueAsync(node.Name, null).ConfigureAwait(false);
                    break;
                case NodeTypes.Value:
                    await writer.WriteValueAsync(node.Name, ((NodeValue)node).Value).ConfigureAwait(false);
                    break;
                case NodeTypes.List:
                    await writer.BeginListAsync(node.Name).ConfigureAwait(false);

                    foreach(var child in ((NodeList)node).Children()) {
                        await CopyToRecursiveAsync(child, writer).ConfigureAwait(false);
                    }
                    await writer.EndListAsync().ConfigureAwait(false);
                    break;
                case NodeTypes.Map:
                    await writer.BeginMapAsync(node.Name).ConfigureAwait(false);

                    foreach(var child in ((NodeMap)node).Children()) {
                        await CopyToRecursiveAsync(child, writer).ConfigureAwait(false);
                    }

                    await writer.EndMapAsync().ConfigureAwait(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
