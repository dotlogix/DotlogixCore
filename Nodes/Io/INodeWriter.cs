namespace DotLogix.Core.Nodes.Io {
    public interface INodeWriter {
        void BeginMap(string name = null);
        void EndMap();

        void BeginList(string name = null);
        void EndList();

        void WriteValue(object value, string name = null);
        void AutoComplete();
        void ExecuteOperation(NodeIoOp operation);
    }
}