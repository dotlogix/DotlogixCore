namespace DotLogix.Core.Nodes.Processor {
    public interface INodeWriter {
        void BeginMap();
        void BeginMap(string name);
        void EndMap();

        void BeginList();
        void BeginList(string name);
        void EndList();

        void WriteValue(string name, object value);
        void WriteValue(object value);

        void AutoComplete();

        void Execute(NodeOperation operation);
    }
}