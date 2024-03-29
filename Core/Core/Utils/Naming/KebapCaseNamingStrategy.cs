using System.Text;

namespace DotLogix.Core.Utils.Naming; 

/// ///
/// <summary>
///     A class to sanitize and convert string values to their corresponding kebap-case representation
/// </summary>
public class KebapCaseNamingStrategy : NamingStrategyBase {
    /// <inheritdoc />
    protected override string RewriteValue(string value, StringBuilder stringBuilder) {
        var isFirst = true;
        foreach (var word in ExtractWords(value)) {
            if (word.Array == null)
                continue;

            var wordStart = word.Offset;
            var wordEnd = wordStart + word.Count;

            if (isFirst)
                isFirst = false;
            else
                stringBuilder.Append('-');

            for (var i = wordStart; i < wordEnd; i++)
                stringBuilder.Append(char.ToLowerInvariant(word.Array[i]));
        }

        return stringBuilder.ToString();
    }
}