namespace DotLogix.Core.Nodes.Io {
    public struct NodeIoOperation {
        public NodeIoState NextState;
        public NodeIoOpCodes OpCode;
        public NodeIoOpCodes AllowedNextOpCodes;
    }
}