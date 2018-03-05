using System;

namespace DotLogix.Core.Nodes.Processor {
    public class NodeWriter : NodeWriterBase {
        protected NodeCollection CurrentNodeCollection;
        public Node Root { get; private set; }

        public override void BeginMap(string name) {
            CheckName(name);

            var map = new NodeMap(name);
            PushContainer(NodeContainerType.Map);
            if(Root == null) 
                Root = map;
            else
                CurrentNodeCollection.AddChild(map);

            CurrentNodeCollection = map;
        }

        public override void EndMap() {
            PopExpectedContainer(NodeContainerType.Map);
            CurrentNodeCollection = CurrentNodeCollection.Ancestor;
        }

        public override void BeginList(string name) {
            CheckName(name);

            var list = new NodeList(name);
            PushContainer(NodeContainerType.List);
            if(Root == null)
                Root = list;
            else
                CurrentNodeCollection.AddChild(list);

            CurrentNodeCollection = list;
        }

        public override void EndList() {
            PopExpectedContainer(NodeContainerType.List);
            CurrentNodeCollection = CurrentNodeCollection.Ancestor;
        }

        public override void WriteValue(string name, object value) {
            CheckName(name);

            var val = new NodeValue(name, value);
            if(Root == null)
                Root = val;
            else
                CurrentNodeCollection.AddChild(val);
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
    }
}