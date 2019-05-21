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

        public override ValueTask BeginMapAsync(string name) {
            CheckName(name);

            var map = new NodeMap();
            if(Root == null)
                Root = map;
            else
                AddChild(name, map);

            CurrentNodeCollection = map;
            PushContainer(NodeContainerType.Map);
            return default;
        }

        public override ValueTask EndMapAsync() {
            PopExpectedContainer(NodeContainerType.Map);
            CurrentNodeCollection = CurrentNodeCollection.Ancestor;
            return default;
        }

        public override ValueTask BeginListAsync(string name) {
            CheckName(name);

            var list = new NodeList();
            if(Root == null)
                Root = list;
            else
                AddChild(name, list);

            CurrentNodeCollection = list;
            PushContainer(NodeContainerType.List);
            return default;
        }

        public override ValueTask EndListAsync() {
            PopExpectedContainer(NodeContainerType.List);
            CurrentNodeCollection = CurrentNodeCollection.Ancestor;
            return default;
        }

        public override ValueTask WriteValueAsync(string name, object value) {
            CheckName(name);

            var val = new NodeValue(value);
            if(Root == null)
                Root = val;
            else
                AddChild(name, val);
            return default;
        }

        private void CheckName(string name) {
            switch(CurrentContainer) {
                case NodeContainerType.Map:
                    if(name == null)
                        throw new ArgumentNullException(nameof(name), $"Name can not be null in the current container {CurrentContainer}");
                    break;
                case NodeContainerType.List:
                    if(name != null)
                        throw new ArgumentException(nameof(name), $"Name can not have a value in the current container {CurrentContainer}");
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
            switch(CurrentContainer) {
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
