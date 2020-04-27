using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Utils;

namespace DotLogix.Core.Nodes {
    public class MemberSettings : TypeSettings {
        public DynamicAccessor Accessor { get; set; }
        public string Name { get; set; }
        public int? Order { get; set; }
    }
}