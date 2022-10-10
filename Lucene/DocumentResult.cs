using Lucene.Net.Documents;

namespace DotLogix.Core.Lucene {
    /// <summary>
    /// A helper struct to represent document results and their score
    /// </summary>
    public struct DocumentResult {
        /// <summary>
        /// The score retrieved from lucene
        /// </summary>
        public readonly float Score;

        /// <summary>
        /// The document
        /// </summary>
        public readonly Document Document;

        /// <summary>
        /// Creates a new instance of <see cref="DocumentResult"/>
        /// </summary>
        public DocumentResult(float score, Document document) {
            Score = score;
            Document = document;
        }
    }
}