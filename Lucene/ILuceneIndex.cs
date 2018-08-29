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
using Version = Lucene.Net.Util.Version;
#endregion

namespace DotLogix.Lucene {
    public interface ILuceneIndex : IDisposable {
        Analyzer Analyzer { get; }
        Directory Directory { get; }
        string IndexPath { get; }
        Version Version { get; }

        void AddDocument(Document document, bool commit = true);
        void UpdateDocument(Term matchTerm, Document document, bool commit = true);
        void RemoveDocument(Term matchTerm, bool commit = true);
        DocumentResult QueryResult(Query query);
        IEnumerable<DocumentResult> QueryResults(Query query, int count, bool order = true);
    }
}
