// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeWriter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  16.07.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Threading.Tasks;
#endregion

namespace DotLogix.Core.Nodes.Processor {
    public class NodeWriter : NodeWriterBase {
        protected NodeContainer CurrentNodeCollection;
        public NodeWriter(ConverterSettings converterSettings = null) : base(converterSettings) { }
        public Node Root { get; private set; }
        public bool IsComplete => Root != null && ContainerStack.Count == 0;

        #region Async

        public override ValueTask WriteBeginMapAsync() {
            CheckName(CurrentName);

            var map = new NodeMap();
            if(Root == null)
                Root = map;
            else
                AddChild(CurrentName, map);
            CurrentName = null;

            CurrentNodeCollection = map;
            ContainerStack.Push(NodeContainerType.Map);
            return default;
        }

        public override ValueTask WriteEndMapAsync() {
            ContainerStack.PopExpected(NodeContainerType.Map);
            CurrentNodeCollection = CurrentNodeCollection.Ancestor;
            return default;
        }

        public override ValueTask WriteBeginListAsync() {
            CheckName(CurrentName);

            var list = new NodeList();
            if(Root == null)
                Root = list;
            else
                AddChild(CurrentName, list);
            CurrentName = null;

            CurrentNodeCollection = list;
            ContainerStack.Push(NodeContainerType.List);
            return default;
        }

        public override ValueTask WriteEndListAsync() {
            ContainerStack.PopExpected(NodeContainerType.List);
            CurrentNodeCollection = CurrentNodeCollection.Ancestor;
            return default;
        }

        public override ValueTask WriteValueAsync(object value) {
            CheckName(CurrentName);

            var val = new NodeValue(value);
            if(Root == null)
                Root = val;
            else
                AddChild(CurrentName, val);
            CurrentName = null;
            return default;
        }

        #endregion

        private void CheckName(string name) {
            switch(ContainerStack.Current) {
                case NodeContainerType.Map:
                    if(name == null)
                        throw new ArgumentNullException(nameof(name), $"Name can not be null in the current container {ContainerStack.Current}");
                    break;
                case NodeContainerType.List:
                    if(name != null)
                        throw new ArgumentException(nameof(name), $"Name can not have a value in the current container {ContainerStack.Current}");
                    break;
                case NodeContainerType.None:
                    if(Root != null)
                        throw new InvalidOperationException("Can not begin a map because there is not container to add the node");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AddChild(string name, Node node) {
            switch(ContainerStack.Current) {
                case NodeContainerType.List:
                    ((NodeList)CurrentNodeCollection).AddChild(node);
                    break;
                case NodeContainerType.Map:
                    ((NodeMap)CurrentNodeCollection).AddChild(name, node);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
