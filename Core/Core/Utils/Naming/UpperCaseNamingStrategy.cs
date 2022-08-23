using System.Text;

namespace DotLogix.Core.Utils.Naming; 

/// ///
/// <summary>
///     A class to sanitize and convert string values to their corresponding UPPERCASE representation
/// </summary>
public class UpperCaseNamingStrategy : NamingStrategyBase {
    /// <inheritdoc />
    protected override string RewriteValue(string value, StringBuilder stringBuilder) {
        foreach (var word in ExtractWords(value)) {
            if (word.Array == null)
                continue;

            var wordStart = word.Offset;
            var wordEnd = wordStart + word.Count;

            for (var i = wordStart; i < wordEnd; i++)
                stringBuilder.Append(char.ToUpperInvariant(word.Array[i]));
        }

        return stringBuilder.ToString();
    }
}