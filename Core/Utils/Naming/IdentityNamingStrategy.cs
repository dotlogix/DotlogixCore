using System.Text;

namespace DotLogix.Core.Utils.Naming {
    public class IdentityNamingStrategy : NamingStrategyBase {
        protected override string RewriteValue(string value, StringBuilder stringBuilder) {
            return value;
        }
    }
}
