using System.Text;

namespace DotLogix.Core.Utils.Naming
{
    /// /// <summary>
    /// A class to sanitize and convert string values to their corresponding lowercase representation
    /// </summary>
    public class LowerCaseNamingStrategy : NamingStrategyBase
    {
        /// <inheritdoc />
        protected override string RewriteValue(string value, StringBuilder stringBuilder) {
            foreach (var word in ExtractWords(value)) {
                var wordStart = word.Offset;
                var wordEnd = wordStart + word.Count;

                for (var i = wordStart; i < wordEnd; i++)
                    stringBuilder.Append(char.ToLowerInvariant(word.Buffer[i]));
            }

            return stringBuilder.ToString();
        }
    }
}