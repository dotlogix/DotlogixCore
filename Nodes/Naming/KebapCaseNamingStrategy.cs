using System.Text;

namespace DotLogix.Core.Nodes {
    /// <summary>
    /// A kebap-case naming strategy
    /// </summary>
    public class KebapCaseNamingStrategy : NamingStrategyBase {
        /// <inheritdoc />
        protected override bool TransformIfRequired(string name, StringBuilder builder) {
            for(var i = 0; i < name.Length; i++) {
                var chr = name[i];
                var lc = char.ToLowerInvariant(chr);

                if(i > 0 && chr != lc)
                    builder.Append('-');
                builder.Append(lc);
            }

            return true;
        }
    }
}