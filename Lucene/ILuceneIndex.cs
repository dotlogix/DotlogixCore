// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ILuceneIndex.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  19.08.2018
// LastEdited:  19.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
#endregion

namespace DotLogix.Core.Lucene {
    /// <summary>
    /// An interface to represent a lucene index
    /// </summary>
    public interface ILuceneIndex : IDisposable {
        /// <summary>
        /// The searcher manager
        /// </summary>
        SearcherManager SearcherManager { get; }

        /// <summary>
        /// The reader manager
        /// </summary>
        ReaderManager ReaderManager { get; }

        /// <summary>
        /// The index writer
        /// </summary>
        IndexWriter IndexWriter { get; }

        /// <summary>
        /// The analyzer
        /// </summary>
        Analyzer Analyzer { get; }

        /// <summary>
        /// The directory
        /// </summary>
        Directory Directory { get; }

        /// <summary>
        /// The index lucene version
        /// </summary>
        LuceneVersion Version { get; }


        /// <summary>
        /// Add a single document to the index
        /// </summary>
        /// <param name="document">The document to add</param>
        /// <param name="commit">Determines if all changes should be committed directly</param>
        void AddDocument(Document document, bool commit = true);

        /// <summary>
        /// Update a single document of the index
        /// </summary>
        /// <param name="matchTerm">The term to match</param>
        /// <param name="document">The new document</param>
        /// <param name="commit">Determines if all changes should be committed directly</param>
        void UpdateDocument(Term matchTerm, Document document, bool commit = true);

        /// <summary>
        /// Remove a single document from the index
        /// </summary>
        /// <param name="matchTerm">The term to match</param>
        /// <param name="commit">Determines if all changes should be committed directly</param>
        void RemoveDocument(Term matchTerm, bool commit = true);

        /// <summary>
        /// Get a single document result
        /// </summary>
        /// <param name="query">A query to match</param>
        /// <returns></returns>
        DocumentResult QueryResult(Query query);

        /// <summary>
        /// Get a range of document results
        /// </summary>
        /// <param name="query">A query to match</param>
        /// <param name="count">The maximum amount of documents</param>
        /// <param name="order">Determines if the documents should be ordered</param>
        IEnumerable<DocumentResult> QueryResults(Query query, int count, bool order = true);
    }
}
