using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;

namespace DotLogix.Lucene {
    public class LuceneIndex : ILuceneIndex {
        private Analyzer _analyzer;
        private Directory _directory;

        public LuceneIndex(Version version, string indexPath) {
            Version = version;
            IndexPath = indexPath;
        }

        public void Dispose() {
            _analyzer?.Dispose();
            _directory?.Dispose();
            _analyzer = null;
            _directory = null;
        }

        public Analyzer Analyzer => _analyzer ?? (_analyzer = CreateAnalyzer());
        public Directory Directory => _directory ?? (_directory = FSDirectory.Open(IndexPath));
        public Version Version { get; }
        public string IndexPath { get; }
        #region Index
        public void CreateNew() {
            using(var writer = CreateIndexWriter(true)) {
                writer.Commit();
            }
        }
        #endregion
        #region Documents
        public void AddDocument(Document document, bool commit = true) {
            using(var writer = CreateIndexWriter()) {
                writer.AddDocument(document);
                writer.Optimize();
                if(commit)
                    writer.Commit();
            }
        }

        public void UpdateDocument(Term matchTerm, Document document, bool commit = true) {
            using(var writer = CreateIndexWriter()) {
                writer.UpdateDocument(matchTerm, document);
                writer.Optimize();
                if(commit)
                    writer.Commit();
            }
        }

        public void RemoveDocument(Term matchTerm, bool commit = true) {
            using(var writer = CreateIndexWriter()) {
                writer.DeleteDocuments(matchTerm);
                writer.Optimize();
                if(commit)
                    writer.Commit();
            }
        }
        #endregion

        #region Query
        public DocumentResult QueryResult(Query query) {
            return QueryResults(query, 1).FirstOrDefault();
        }


        public IEnumerable<DocumentResult> QueryResults(Query query, int count, bool order = true) {
            var collector = TopScoreDocCollector.Create(count, order);
            var results = new List<DocumentResult>();
            using(var searcher = CreateIndexSearcher()) {
                searcher.Search(query, collector);

                foreach(var scoreDoc in collector.TopDocs().ScoreDocs) {
                    var doc = searcher.Doc(scoreDoc.Doc);
                    results.Add(new DocumentResult(scoreDoc.Score, doc));
                }
            }

            return results;
        }
        #endregion

        #region Create
        protected virtual Analyzer CreateAnalyzer() {
            return new StandardAnalyzer(Version);
        }

        protected virtual IndexWriter CreateIndexWriter(bool createIndex=false) {
            return new IndexWriter(Directory, Analyzer, createIndex, IndexWriter.MaxFieldLength.UNLIMITED);
        }

        protected virtual IndexSearcher CreateIndexSearcher() {
            return new IndexSearcher(Directory, true);
        }
        #endregion
    }
}