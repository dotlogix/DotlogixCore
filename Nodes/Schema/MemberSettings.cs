using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Core.Nodes.Schema {
    public class MemberSettings : TypeSettings {
        public string Name { get; set; }
        public int? Order { get; set; }
        public DynamicAccessor Accessor { get; set; }
    }
}