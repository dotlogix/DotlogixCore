using System.Text;

namespace DotLogix.Core.Utils.Naming
{
    /// /// <summary>
    /// A class to sanitize and convert string values to their corresponding camelCase representation
    /// </summary>
    public class CamelCaseNamingStrategy : NamingStrategyBase
    {
        /// <inheritdoc />
        protected override string RewriteValue(string value, StringBuilder stringBuilder) {
            var isFirst = true;
            foreach (var word in ExtractWords(value)) {
                var wordStart = word.Offset;
                var wordEnd = wordStart + word.Count;

                if (isFirst) {
                    isFirst = false;
                }
                else
                {
                    stringBuilder.Append(char.ToUpperInvariant(word[0]));
                    wordStart++;
                }

                for (var i = wordStart; i < wordEnd; i++) {
                    stringBuilder.Append(char.ToLowerInvariant(word.Buffer[i]));
                }
            }

            return stringBuilder.ToString();
        }
    }
}