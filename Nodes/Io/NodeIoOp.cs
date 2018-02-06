namespace DotLogix.Core.Nodes.Io {
    public struct NodeIoOp {
        public object[] Args;
        public NodeIoOpCodes OpCode;

        public NodeIoOp(NodeIoOpCodes opCode, params object[] args) {
            OpCode = opCode;
            Args = args;
        }

        public object GetArg(int index) {
            return Args.Length > index ? Args[index] : default(object);
        }

        public T GetArg<T>(int index) {
            return Args.Length > index ? (T)Args[index] : default(T);
        }

        public override string ToString() {
            return $"{OpCode}({string.Join(", ", Args)})";
        }
    }
}