using System.Text;

namespace DotLogix.Core.Utils.Naming {
    /// /// <summary>
    /// A class to sanitize and convert string values to their corresponding UPPER_SNAKE_CASE representation
    /// </summary>
    public class UpperSnakeCaseNamingStrategy : NamingStrategyBase
    {
        /// <inheritdoc />
        protected override string RewriteValue(string value, StringBuilder stringBuilder) {
            var isFirst = true;
            foreach (var word in ExtractWords(value)) {
                var wordStart = word.Offset;
                var wordEnd = wordStart + word.Count;

                if (isFirst)
                    isFirst = false;
                else
                    stringBuilder.Append('_');

                for (var i = wordStart; i < wordEnd; i++)
                    stringBuilder.Append(char.ToUpperInvariant(word.Buffer[i]));
            }

            return stringBuilder.ToString();
        }
    }
}