namespace DotLogix.Core.Extensions {
    public enum StringSimilarity {
        /// <summary>
        ///    Calculates the number of positions at which the corresponding strings are different. (MUST be of equal length)
        /// </summary>
        Hamming,
        /// <summary>
        /// Calculates the minimum number of single-character edits (i.e. insertions, deletions or substitutions) required to change one word into the other
        /// </summary>
        Levenshtein,
        /// <summary>
        /// Calculates the minimum number of single-character edits (i.e. insertions, deletions, substitutions or transpositions) required to change one word into the other
        /// </summary>
        DamerauLevenshtein,
        /// <summary>
        /// Calculates the amount of matching trigrams
        /// </summary>
        Trigrams
    }
}