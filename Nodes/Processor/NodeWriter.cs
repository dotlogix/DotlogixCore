using System;

namespace DotLogix.Core.Nodes.Processor {
    public class NodeWriter : NodeWriterBase {
        protected NodeContainer CurrentNodeCollection;
        public Node Root { get; private set; }

        public override void BeginMap(string name) {
            CheckName(name);

            var map = new NodeMap();
            if(Root == null) 
                Root = map;
            else
                AddChild(name, map);

            CurrentNodeCollection = map;
            PushContainer(NodeContainerType.Map);
        }

        public override void EndMap() {
            PopExpectedContainer(NodeContainerType.Map);
            CurrentNodeCollection = CurrentNodeCollection.Ancestor;
        }

        public override void BeginList(string name) {
            CheckName(name);

            var list = new NodeList();
            if(Root == null)
                Root = list;
            else
                AddChild(name, list);

            CurrentNodeCollection = list;
            PushContainer(NodeContainerType.List);
        }

        public override void EndList() {
            PopExpectedContainer(NodeContainerType.List);
            CurrentNodeCollection = CurrentNodeCollection.Ancestor;
        }

        public override void WriteValue(string name, object value) {
            CheckName(name);

            var val = new NodeValue(value);
            if(Root == null)
                Root = val;
            else
                AddChild(name, val);
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

        private void AddChild(string name, Node node)
        {
            switch (CurrentContainer)
            {
                case NodeContainerType.List:
                    ((NodeList) CurrentNodeCollection).AddChild(node);
                    break;
                case NodeContainerType.Map:
                    ((NodeMap) CurrentNodeCollection).AddChild(name, node);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}