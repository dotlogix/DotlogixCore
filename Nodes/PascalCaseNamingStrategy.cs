using System.Text;

namespace DotLogix.Core.Nodes {
    public class PascalCaseNamingStrategy : NamingStrategyBase {
        protected override bool TransformIfRequired(string name, StringBuilder builder) {
            var first = name[0];
            var firstUc = char.ToUpperInvariant(first);
            if(first != firstUc) {
                builder.Append(firstUc);
                builder.Append(name, 1, name.Length - 1);
                return true;
            }
            return false;
        }
    }
}