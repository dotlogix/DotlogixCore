namespace DotLogix.Core.Nodes.Io {
    public interface INodeWriter {
        INodeWriter CopyFrom(INodeReader reader);

        INodeWriter BeginMap(string name = null);
        INodeWriter EndMap();

        INodeWriter BeginList(string name = null);
        INodeWriter EndList();

        INodeWriter WriteValue(object value, string name = null);
        INodeWriter AutoComplete();
        INodeWriter ExecuteOperation(NodeIoOp operation);
    }
}