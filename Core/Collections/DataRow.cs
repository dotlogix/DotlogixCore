using System;
using System.Collections.Generic;
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Collections {
	public struct DataRow {
		public object[] Values { get; }
		public string[] Headers { get; }
		private IDictionary<string, int> HeaderMap { get; }

		public DataRow(string[] headers, IDictionary<string, int> headerMap, object[] values) {
			Values = values;
			Headers = headers;
			HeaderMap = headerMap;
		}

		public int GetIndexOf(string name) { return HeaderMap.TryGetValue(name, out var idx) ? idx : -1; }

        public bool TryGetIndexOf(string name, out int index) {
            return HeaderMap.TryGetValue(name, out index);
        }

		public bool TryGetValue(int index, out object value) {
			if (index < 0 || index >= Values.Length) {
				value = default;
				return false;
			}

			value = Values[index];
			return true;
		}

		public bool TryGetValueAs<T>(int index, out T value) {
			if (TryGetValue(index, out var obj) && obj.TryConvertTo(out value))
				return true;

			value = default;
			return false;
		}

		public bool TryGetValueAs(int index, Type type, out object value) {
			if (TryGetValue(index, out var obj) && obj.TryConvertTo(type, out value))
				return true;

			value = default;
			return false;
		}

		public object GetValue(int index, object defaultValue) {
			return TryGetValue(index, out var value)
				       ? value
				       : defaultValue;
		}

		public T GetValueAs<T>(int index, T defaultValue) {
			return TryGetValue(index, out var obj) && obj.TryConvertTo(out T value)
				       ? value
				       : defaultValue;
		}

		public object GetValueAs(int index, Type type, object defaultValue) {
			return TryGetValue(index, out var obj) && obj.TryConvertTo(type, out var value)
				       ? value
				       : defaultValue;
		}


		public bool TryGetValue(string name, out object value) {
			if (TryGetIndexOf(name, out var index))
				return TryGetValue(index, out value);
			value = default;
			return false;
		}

		public bool TryGetValueAs<T>(string name, out T value) {
			if (TryGetIndexOf(name, out var index))
				return TryGetValueAs(index, out value);
			value = default;
			return false;
		}

		public bool TryGetValueAs(string name, Type type, out object value) {
			if (TryGetIndexOf(name, out var index))
				return TryGetValueAs(index, type, out value);
			value = default;
			return false;
		}

		public object GetValue(string name, object defaultValue) {
			return TryGetIndexOf(name, out var index)
				       ? GetValue(index, defaultValue)
				       : defaultValue;
		}

		public T GetValueAs<T>(string name, T defaultValue) {
			return TryGetIndexOf(name, out var index)
				       ? GetValueAs(index, defaultValue)
				       : defaultValue;
		}

		public object GetValueAs(string name, Type type, object defaultValue) {
			return TryGetIndexOf(name, out var index)
				       ? GetValueAs(index, type, defaultValue)
				       : defaultValue;
		}
	}
}
