// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeReader.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Nodes.Processor {
    public class NodeReader : NodeReaderBase {
        public Node Node { get; }

        public NodeReader(Node node) {
            Node = node;
            Operations = EnumerateRecursive(node).GetEnumerator();
        }
        
        protected override ValueTask<NodeOperation?> ReadNextAsync() {
            return Operations.MoveNext()
                       ? new ValueTask<NodeOperation?>(Operations.Current)
                       : default;
        }

        public IEnumerator<NodeOperation> Operations { get; set; }

        /// <inheritdoc />
        protected override void Dispose(bool disposing) {
            Operations.Dispose();
            base.Dispose(disposing);
        }

        protected IEnumerable<NodeOperation> EnumerateRecursive(Node initial) {
            switch(initial.Type) {
                case NodeTypes.Empty:
                    yield return new NodeOperation(NodeOperationTypes.Value, initial.Name);
                    yield break;
                case NodeTypes.Value:
                    yield return new NodeOperation(NodeOperationTypes.Value, initial.Name, ((NodeValue)initial).Value);
                    yield break;
            }


            var stack = new Stack<(NodeTypes, IEnumerator<Node>)>();
            var enumerator = initial.CreateEnumerable().GetEnumerator();
            var containerType = initial.Type;

            try {
                while (true) {
                    if (enumerator.MoveNext()) {
                        var currentNode = enumerator.Current;
                        if (currentNode == null)
                            continue;

                        switch (currentNode.Type) {
                            case NodeTypes.Empty:
                                yield return new NodeOperation(NodeOperationTypes.Value, currentNode.Name);
                                break;
                            case NodeTypes.Value:
                                yield return new NodeOperation(NodeOperationTypes.Value, currentNode.Name, ((NodeValue)currentNode).Value);
                                break;
                            case NodeTypes.List:
                            case NodeTypes.Map:
                                if (currentNode.Type == NodeTypes.List) {
                                    yield return new NodeOperation(NodeOperationTypes.BeginList, currentNode.Name);
                                } else {
                                    yield return new NodeOperation(NodeOperationTypes.BeginMap, currentNode.Name);
                                }

                                var childEnumerator = ((NodeContainer)currentNode).Children().GetEnumerator();
                                stack.Push((containerType, enumerator));
                                enumerator = childEnumerator;
                                containerType = currentNode.Type;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    } else if (stack.Count > 0) {
                        enumerator.Dispose();

                        switch(containerType) {
                            case NodeTypes.List:
                                yield return new NodeOperation(NodeOperationTypes.EndList);
                                break;
                            case NodeTypes.Map:
                                yield return new NodeOperation(NodeOperationTypes.EndMap);
                                break;
                        }
                        (containerType, enumerator) = stack.Pop();
                    } else {
                        switch (containerType) {
                            case NodeTypes.List:
                                yield return new NodeOperation(NodeOperationTypes.EndList);
                                break;
                            case NodeTypes.Map:
                                yield return new NodeOperation(NodeOperationTypes.EndMap);
                                break;
                        }
                        yield break;
                    }
                }
            } finally {
                enumerator.Dispose();

                while (stack.Count > 0) // Clean up in case of an exception.
                {
                    (_, enumerator) = stack.Pop();
                    enumerator.Dispose();
                }
            }
        }
    }
}
