namespace DotLogix.Core.Nodes.Processor {
    public enum NodeOperationTypes {
        BeginMap = 1 << 0,
        EndMap = 1 << 1,
        BeginList = 1 << 2,
        EndList = 1 << 3,
        WriteValue = 1 << 4,
        AutoComplete = 1 << 5,
        BeginAny = BeginMap | BeginList | WriteValue
    }
}