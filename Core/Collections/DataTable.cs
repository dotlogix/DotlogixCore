// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  DataTable.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 22.08.2020 13:51
// LastEdited:  26.09.2021 22:27
// ==================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DotLogix.Core.Collections {
	public class DataTable : IEnumerable<DataRow> {
		private readonly IEnumerable<object[]> _rowValues;
		public string[] Headers { get; }
		private IDictionary<string, int> HeaderMap { get; }

		public DataTable(string[] headers, IEnumerable<object[]> rowValues) {
			_rowValues = rowValues;
			Headers = headers;
			HeaderMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
			for (var i = 0; i < headers.Length; i++)
				HeaderMap.Add(headers[i], i);
		}


		public IEnumerator<DataRow> GetEnumerator() {
			return _rowValues.Select(v => new DataRow(Headers, HeaderMap, v)).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
	}
}
