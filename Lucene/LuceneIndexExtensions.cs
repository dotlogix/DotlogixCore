using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Documents;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;

namespace DotLogix.Lucene {
    public static class LuceneIndexExtensions {
        public static QueryParser CreateQueryParser(this ILuceneIndex index, string field) {
            return new QueryParser(index.Version, field, index.Analyzer);
        }

        public static MultiFieldQueryParser CreateQueryParser(this ILuceneIndex index, string[] fields, IDictionary<string, float> boosts = null) {
            return new MultiFieldQueryParser(index.Version, fields, index.Analyzer, boosts);
        }

        public static Document QueryDocument(this ILuceneIndex index, Query query) {
            return index.QueryResult(query).Document;
        }

        public static IEnumerable<Document> QueryDocuments(this ILuceneIndex index, Query query, int count, bool order = true) {
            return index.QueryResults(query, count, order).Select(r => r.Document);
        }
    }
}