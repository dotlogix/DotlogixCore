using System.Text;

namespace DotLogix.Core.Nodes {
    public class CamelCaseNamingStrategy : NamingStrategyBase {
        protected override bool TransformIfRequired(string name, StringBuilder builder) {
            var first = name[0];
            var firstLc = char.ToLowerInvariant(first);
            if(first != firstLc) {
                builder.Append(firstLc);
                builder.Append(name, 1, name.Length - 1);
                return true;
            }
            return false;
        }
    }
}