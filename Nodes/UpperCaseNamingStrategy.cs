using System.Text;

namespace DotLogix.Core.Nodes {
    public class UpperCaseNamingStrategy : NamingStrategyBase {
        protected override bool TransformIfRequired(string name, StringBuilder builder) {
            foreach(var chr in name)
                builder.Append(char.ToUpperInvariant(chr));
            return true;
        }
    }
}