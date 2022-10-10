using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Documents;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;

namespace DotLogix.Core.Lucene {
    /// <summary>
    ///     A static class providing extension methods for <see cref="ILuceneIndex" />
    /// </summary>
    public static class LuceneIndexExtensions {
        /// <summary>
        /// Creates a query parser
        /// </summary>
        public static QueryParser CreateQueryParser(this ILuceneIndex index, string field) {
            return new QueryParser(index.Version, field, index.Analyzer);
        }

        /// <summary>
        /// Creates a query parser
        /// </summary>
        public static MultiFieldQueryParser CreateQueryParser(this ILuceneIndex index, string[] fields, IDictionary<string, float> boosts = null) {
            return new MultiFieldQueryParser(index.Version, fields, index.Analyzer, boosts);
        }

        /// <summary>
        /// Get a single document
        /// </summary>
        /// <param name="index">The index</param>
        /// <param name="query">A query to match</param>
        /// <returns></returns>
        public static Document QueryDocument(this ILuceneIndex index, Query query) {
            return index.QueryResult(query).Document;
        }

        /// <summary>
        /// Get a range of documents
        /// </summary>
        /// <param name="index">The index</param>
        /// <param name="query">A query to match</param>
        /// <param name="count">The maximum amount of documents</param>
        /// <param name="order">Determines if the documents should be ordered</param>
        public static IEnumerable<Document> QueryDocuments(this ILuceneIndex index, Query query, int count, bool order = true) {
            return index.QueryResults(query, count, order).Select(r => r.Document);
        }
    }
}