using System.Text;

namespace DotLogix.Core.Nodes {
    /// <summary>
    /// A lowercase naming strategy
    /// </summary>
    public class LowerCaseNamingStrategy : NamingStrategyBase {
        /// <inheritdoc />
        protected override bool TransformIfRequired(string name, StringBuilder builder) {
            foreach(var chr in name)
                builder.Append(char.ToLowerInvariant(chr));
            return true;
        }
    }
}