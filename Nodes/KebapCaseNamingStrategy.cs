using System.Text;

namespace DotLogix.Core.Nodes {
    public class KebapCaseNamingStrategy : NamingStrategyBase {
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