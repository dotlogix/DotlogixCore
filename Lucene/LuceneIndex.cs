#region
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
#endregion

namespace DotLogix.Core.Lucene {
    /// <summary>
    ///     An implementation of the <see cref="ILuceneIndex" /> interface
    /// </summary>
    public abstract class LuceneIndex : ILuceneIndex {
        private Analyzer _analyzer;


        private IndexWriter _indexWriter;
        private SearcherManager _searcherManager;
        private ReaderManager _readerManager;
        private Directory _directory;


        /// <inheritdoc />
        public SearcherManager SearcherManager => _searcherManager ?? (_searcherManager = CreateSearcherManager());
        /// <inheritdoc />
        public ReaderManager ReaderManager => _readerManager ?? (_readerManager = CreateReaderManager());
        /// <inheritdoc />
        public IndexWriter IndexWriter => _indexWriter ?? (_indexWriter = CreateIndexWriter());
        /// <inheritdoc />
        public Analyzer Analyzer => _analyzer ?? (_analyzer = CreateAnalyzer());
        /// <inheritdoc />
        public Directory Directory => _directory ?? (_directory = CreateDirectory());

        /// <summary>
        ///     Creates a new instance of <see cref="LuceneIndex" />
        /// </summary>
        public LuceneIndex(LuceneVersion version) {
            Version = version;
        }


        /// <inheritdoc />

        /// <inheritdoc />
        public LuceneVersion Version { get; }


        #region Documents
        /// <inheritdoc />
        public virtual void AddDocument(Document document, bool commit = true) {
            using(var writer = CreateIndexWriter()) {
                writer.AddDocument(document);
                writer.Commit();
            }
        }

        /// <inheritdoc />
        public virtual void UpdateDocument(Term matchTerm, Document document, bool commit = true) {
            var writer = IndexWriter;
            writer.UpdateDocument(matchTerm, document);
            if(commit)
                writer.Commit();
        }

        /// <inheritdoc />
        public virtual void RemoveDocument(Term matchTerm, bool commit = true) {
            using(var writer = CreateIndexWriter()) {
                writer.DeleteDocuments(matchTerm);
                if(commit)
                    writer.Commit();
            }
        }
        #endregion

        #region Query
        /// <inheritdoc />
        public virtual DocumentResult QueryResult(Query query) {
            return QueryResults(query, 1).FirstOrDefault();
        }


        /// <inheritdoc />
        public virtual IEnumerable<DocumentResult> QueryResults(Query query, int count, bool order = true) {
            var collector = TopScoreDocCollector.Create(count, order);
            List<DocumentResult> results;
            IndexSearcher searcher = null;
            try {
                searcher = SearcherManager.Acquire();
                searcher.Search(query, collector);

                var topDocs = collector.GetTopDocs();
                results = topDocs.ScoreDocs.Select(scoreDoc => new DocumentResult(scoreDoc.Score, searcher.Doc(scoreDoc.Doc))).ToList();
            } finally {
                if(searcher != null)
                    SearcherManager.Release(searcher);
            }

            return results;
        }
        #endregion

        #region Create
        /// <summary>
        ///     Create a new <see cref="Analyzer"/>
        /// </summary>
        /// <returns></returns>
        protected virtual Analyzer CreateAnalyzer() {
            return new StandardAnalyzer(Version);
        }

        /// <summary>
        /// Create a new <see cref="IndexWriter"/>
        /// </summary>
        protected virtual IndexWriter CreateIndexWriter(OpenMode openMode = OpenMode.CREATE_OR_APPEND) {
            return new IndexWriter(Directory, new IndexWriterConfig(Version, Analyzer) {OpenMode = openMode});
        }

        /// <summary>
        /// Create a new <see cref="Directory"/>
        /// </summary>
        protected abstract Directory CreateDirectory();

        /// <summary>
        /// Create a new <see cref="SearcherManager"/>
        /// </summary>
        protected virtual SearcherManager CreateSearcherManager() {
            return new SearcherManager(IndexWriter, true, new SearcherFactory());
        }

        /// <summary>
        /// Create a new <see cref="ReaderManager"/>
        /// </summary>
        protected virtual ReaderManager CreateReaderManager() {
            return new ReaderManager(IndexWriter, true);
        }
        #endregion

        /// <inheritdoc />
        public virtual void Dispose() {
            _searcherManager?.Dispose();
            _readerManager?.Dispose();
            _indexWriter?.Dispose();
            _analyzer?.Dispose();
            _directory?.Dispose();
        }
    }
}
