using System.Text;

namespace DotLogix.Core.Nodes {
    public class LowerCaseNamingStrategy : NamingStrategyBase {
        protected override bool TransformIfRequired(string name, StringBuilder builder) {
            foreach(var chr in name)
                builder.Append(char.ToLowerInvariant(chr));
            return true;
        }
    }
}