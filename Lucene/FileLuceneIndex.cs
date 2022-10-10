// ==================================================
// Copyright 2019(C) , DotLogix
// File:  FileBasedLuceneIndex.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  ..
// LastEdited:  07.06.2019
// ==================================================

using Lucene.Net.Store;
using Lucene.Net.Util;

namespace DotLogix.Core.Lucene {
    /// <summary>
    ///     An file based implementation of the <see cref="ILuceneIndex" /> interface
    /// </summary>
    public class FileLuceneIndex : LuceneIndex {

        /// <summary>
        /// The index path
        /// </summary>
        public string IndexPath { get; }

        /// <summary>
        ///     Creates a new instance of <see cref="FileLuceneIndex" />
        /// </summary>
        public FileLuceneIndex(LuceneVersion version, string indexPath) : base(version) {
            IndexPath = indexPath;
        }

        /// <inheritdoc />
        protected override Directory CreateDirectory()
        {
            return FSDirectory.Open(IndexPath);
        }
    }
    
    /// <summary>
    ///     An file based implementation of the <see cref="ILuceneIndex" /> interface
    /// </summary>
    public class RamLuceneIndex : LuceneIndex {

        /// <summary>
        /// The index path
        /// </summary>
        public string IndexPath { get; }

        /// <summary>
        ///     Creates a new instance of <see cref="FileLuceneIndex" />
        /// </summary>
        public RamLuceneIndex(LuceneVersion version, string indexPath) : base(version) {
            IndexPath = indexPath;
        }

        /// <inheritdoc />
        protected override Directory CreateDirectory()
        {
            return new RAMDirectory();
        }
    }
}