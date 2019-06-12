using System.Text;

namespace DotLogix.Core.Nodes {
    /// <summary>
    /// A UPPERCASE naming strategy
    /// </summary>
    public class UpperCaseNamingStrategy : NamingStrategyBase {
        /// <inheritdoc />
        protected override bool TransformIfRequired(string name, StringBuilder builder) {
            foreach(var chr in name)
                builder.Append(char.ToUpperInvariant(chr));
            return true;
        }
    }
}