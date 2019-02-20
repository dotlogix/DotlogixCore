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
            ValueTask task;
            switch(node.Type) {
                case NodeTypes.Empty:
                    task = writer.WriteValueAsync(node.Name, null);
                    if(task.IsCompleted == false)
                        await task;
                    break;
                case NodeTypes.Value:
                    task = writer.WriteValueAsync(node.Name, ((NodeValue)node).Value);
                    if(task.IsCompleted == false)
                        await task;
                    break;
                case NodeTypes.List:
                    task = writer.BeginListAsync(node.Name);
                    if(task.IsCompleted == false)
                        await task;

                    foreach(var child in ((NodeList)node).Children()) {
                        task = CopyToRecursiveAsync(child, writer);
                        if(task.IsCompleted == false)
                            await task;
                    }
                    task = writer.EndListAsync();
                    if(task.IsCompleted == false)
                        await task;
                    break;
                case NodeTypes.Map:
                    task = writer.BeginMapAsync(node.Name);
                    if(task.IsCompleted == false)
                        await task;

                    foreach(var child in ((NodeMap)node).Children()) {
                        task = CopyToRecursiveAsync(child, writer);
                        if(task.IsCompleted == false)
                            await task;
                    }
                    task = writer.EndMapAsync();
                    if(task.IsCompleted == false)
                        await task;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
