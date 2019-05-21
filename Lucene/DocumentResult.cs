using Lucene.Net.Documents;

namespace DotLogix.Lucene {
    public struct DocumentResult {
        public readonly float Score;
        public readonly Document Document;

        public DocumentResult(float score, Document document) {
            Score = score;
            Document = document;
        }
    }
}