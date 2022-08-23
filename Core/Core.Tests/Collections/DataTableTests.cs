using System.Linq;
using DotLogix.Core.Collections;
using NUnit.Framework;

namespace Core.Tests.Collections {
    [TestFixture]
    public class DataTableTests {
        private const string DefinitivelyNotExistingHeader = "definitively_not_existing_header";

        [SetUp]
        public void Setup() {
        }

        #region IndexOf

        [Test]
        public void GetIndexOf_ExistingHeader_ReturnsCorrectIndex() {
            var table = CreateSimpleTable(out var headers, out _);
            var row = table.First();

            for(var i = 0; i < headers.Length; i++) {
                Assert.That(row.GetIndexOf(headers[i]), Is.EqualTo(i));
            }
        }

        [Test]
        public void GetIndexOf_NonExistingHeader_ReturnsMinusOne() {
            var table = CreateSimpleTable(out _, out _);
            var row = table.First();

            Assert.That(row.GetIndexOf(DefinitivelyNotExistingHeader), Is.EqualTo(-1));
        }

        [Test]
        public void TryGetIndexOf_ExistingHeader_ReturnsTrueAndCorrectIndex() {
            var table = CreateSimpleTable(out var headers, out _);
            var row = table.First();

            for(var i = 0; i < headers.Length; i++) {
                Assert.That(row.TryGetIndexOf(headers[i], out var index), Is.True);
                Assert.That(index, Is.EqualTo(i));
            }
        }

        [Test]
        public void TryGetIndexOf_NonExistingHeader_ReturnsFalseAndZero() {
            var table = CreateSimpleTable(out _, out _);
            var row = table.First();

            Assert.That(row.TryGetIndexOf(DefinitivelyNotExistingHeader, out var index), Is.False);
            Assert.That(index, Is.EqualTo(0));
        }

        #endregion


        #region GetValue

        [Test]
        public void GetValue_ValidIndex_ReturnsCorrectValue() {
            var row = CreateSimpleDataRow(out var headers, out var values);
            var defaultValue = new object();
            for (var i = 0; i < headers.Length; i++) {
                Assert.That(row.GetValue(i, defaultValue), Is.EqualTo(values[i]));
            }
        }

        [Test]
        public void GetValue_OutOfRangeIndex_ReturnsDefaultValue() {
            var row = CreateSimpleDataRow(out _, out var values);

            var defaultValue = new object();
            Assert.That(row.GetValue(int.MinValue, defaultValue), Is.EqualTo(defaultValue));
            Assert.That(row.GetValue(values.Length, defaultValue), Is.EqualTo(defaultValue));
        }

        [Test]
        public void GetValue_ExistingHeader_ReturnsCorrectValue() {
            var row = CreateSimpleDataRow(out var headers, out var values);
            var defaultValue = new object();
            for (var i = 0; i < headers.Length; i++) {
                Assert.That(row.GetValue(headers[i], defaultValue), Is.EqualTo(values[i]));
            }
        }

        [Test]
        public void GetValue_NonExistingHeader_ReturnsDefaultValue() {
            var row = CreateSimpleDataRow(out _, out _);

            var defaultValue = new object();
            Assert.That(row.GetValue(DefinitivelyNotExistingHeader, defaultValue), Is.EqualTo(defaultValue));
        }


        [Test]
        public void TryGetValue_ValidIndex_ReturnsTrueAndCorrectValue() {
            var row = CreateSimpleDataRow(out var headers, out var values);

            for (var i = 0; i < headers.Length; i++) {
                Assert.That(row.TryGetValue(i, out var value), Is.True);
                Assert.That(value, Is.EqualTo(values[i]));
            }
        }

        [Test]
        public void TryGetValue_OutOfRangeIndex_FalseAndDefaultValue() {
            var row = CreateSimpleDataRow(out _, out _);

            Assert.That(row.TryGetValue(int.MinValue, out var value), Is.False);
            Assert.That(value, Is.EqualTo(null));
        }

        [Test]
        public void TryGetValue_ExistingHeader_TrueAndReturnsCorrectValue() {
            var row = CreateSimpleDataRow(out var headers, out var values);

            for (var i = 0; i < headers.Length; i++) {

                Assert.That(row.TryGetValue(headers[i], out var value), Is.True);
                Assert.That(value, Is.EqualTo(values[i]));
            }
        }

        [Test]
        public void TryGetValue_NonExistingHeader_ReturnsFalseAndDefaultValue() {
            var row = CreateSimpleDataRow(out _, out _);

            Assert.That(row.TryGetValue(DefinitivelyNotExistingHeader, out var value), Is.False);
            Assert.That(value, Is.EqualTo(null));
        }
        #endregion

        #region GetValueAs

        [Test]
        public void GetValueAs_ValidIndex_ReturnsCorrectValue() {
            var row = CreateSimpleDataRow(out var headers, out var values);

            var defaultValue = int.MaxValue;
            for (var i = 0; i < headers.Length; i++) {
                Assert.That(row.GetValueAs(i, defaultValue), Is.EqualTo(values[i]));
            }
        }

        [Test]
        public void GetValueAs_OutOfRangeIndex_ReturnsDefaultValue() {
            var row = CreateSimpleDataRow(out _, out var values);

            var defaultValue = int.MaxValue;
            Assert.That(row.GetValueAs(int.MinValue, defaultValue), Is.EqualTo(defaultValue));
            Assert.That(row.GetValueAs(values.Length, defaultValue), Is.EqualTo(defaultValue));
        }

        [Test]
        public void GetValueAs_ExistingHeader_ReturnsCorrectValue() {
            var row = CreateSimpleDataRow(out var headers, out var values);

            var defaultValue = new object();
            for (var i = 0; i < headers.Length; i++) {
                Assert.That(row.GetValueAs(headers[i], defaultValue), Is.EqualTo(values[i]));
            }
        }

        [Test]
        public void GetValueAs_NonExistingHeader_ReturnsDefaultValue() {
            var row = CreateSimpleDataRow(out _, out _);

            var defaultValue = int.MaxValue;
            Assert.That(row.GetValueAs(DefinitivelyNotExistingHeader, defaultValue), Is.EqualTo(defaultValue));
        }
        [Test]
        public void GetValueAsObject_ValidIndex_ReturnsCorrectValue() {
            var row = CreateSimpleDataRow(out var headers, out var values);

            var defaultValue = int.MaxValue;
            for (var i = 0; i < headers.Length; i++) {
                Assert.That(row.GetValueAs(i, typeof(int), defaultValue), Is.EqualTo(values[i]));
            }
        }

        [Test]
        public void GetValueAsObject_OutOfRangeIndex_ReturnsDefaultValue() {
            var row = CreateSimpleDataRow(out _, out var values);

            var defaultValue = int.MaxValue;
            Assert.That(row.GetValueAs(int.MinValue, typeof(int), defaultValue), Is.EqualTo(defaultValue));
            Assert.That(row.GetValueAs(values.Length, typeof(int), defaultValue), Is.EqualTo(defaultValue));
        }

        [Test]
        public void GetValueAsObject_ExistingHeader_ReturnsCorrectValue() {
            var row = CreateSimpleDataRow(out var headers, out var values);

            var defaultValue = new object();
            for (var i = 0; i < headers.Length; i++) {
                Assert.That(row.GetValueAs(headers[i], typeof(int), defaultValue), Is.EqualTo(values[i]));
            }
        }

        [Test]
        public void GetValueAsObject_NonExistingHeader_ReturnsDefaultValue() {
            var row = CreateSimpleDataRow(out _, out _);

            var defaultValue = int.MaxValue;
            Assert.That(row.GetValueAs(DefinitivelyNotExistingHeader, typeof(int), defaultValue), Is.EqualTo(defaultValue));
        }

        [Test]
        public void TryGetValueAs_ValidIndex_ReturnsTrueAndCorrectValue() {
            var row = CreateSimpleDataRow(out var headers, out var values);

            for (var i = 0; i < headers.Length; i++) {
                Assert.That(row.TryGetValueAs<int>(i, out var value), Is.True);
                Assert.That(value, Is.EqualTo(values[i]));
            }
        }

        [Test]
        public void TryGetValueAs_OutOfRangeIndex_FalseAndDefaultValue() {
            var row = CreateSimpleDataRow(out _, out _);

            Assert.That(row.TryGetValueAs<int>(int.MinValue, out var value), Is.False);
            Assert.That(value, Is.EqualTo(0));
        }

        [Test]
        public void TryGetValueAs_ExistingHeader_TrueAndReturnsCorrectValue() {
            var row = CreateSimpleDataRow(out var headers, out var values);

            for (var i = 0; i < headers.Length; i++) {

                Assert.That(row.TryGetValueAs<int>(headers[i], out var value), Is.True);
                Assert.That(value, Is.EqualTo(values[i]));
            }
        }

        [Test]
        public void TryGetValueAs_NonExistingHeader_ReturnsFalseAndDefaultValue() {
            var row = CreateSimpleDataRow(out _, out _);

            Assert.That(row.TryGetValueAs<int>(DefinitivelyNotExistingHeader, out var value), Is.False);
            Assert.That(value, Is.EqualTo(0));
        }

        [Test]
        public void TryGetValueAs_ExistingHeader_AutoConvertsAndReturnCorrectValue() {
            var table = CreateSimpleTable(out var headers, out _);
            var row = table.Skip(1).First();
            
            for(var i = 0; i < headers.Length; i++) {

                Assert.That(row.TryGetValueAs<int>(headers[i], out var value), Is.True);
                Assert.That(value, Is.EqualTo(i));
            }
        }
        
        [Test]
        public void TryGetValueAsObject_ValidIndex_ReturnsTrueAndCorrectValue() {
            var row = CreateSimpleDataRow(out var headers, out var values);

            for (var i = 0; i < headers.Length; i++) {
                Assert.That(row.TryGetValueAs(i, typeof(int), out var value), Is.True);
                Assert.That(value, Is.EqualTo(values[i]));
            }
        }

        [Test]
        public void TryGetValueAsObject_OutOfRangeIndex_FalseAndDefaultValue() {
            var row = CreateSimpleDataRow(out _, out _);

            Assert.That(row.TryGetValueAs(int.MinValue, typeof(int), out var value), Is.False);
            Assert.That(value, Is.Null);
        }

        [Test]
        public void TryGetValueAsObject_ExistingHeader_TrueAndReturnsCorrectValue() {
            var row = CreateSimpleDataRow(out var headers, out var values);

            for (var i = 0; i < headers.Length; i++) {

                Assert.That(row.TryGetValueAs(headers[i], typeof(int), out var value), Is.True);
                Assert.That(value, Is.EqualTo(values[i]));
            }
        }

        [Test]
        public void TryGetValueAsObject_NonExistingHeader_ReturnsFalseAndDefaultValue() {
            var row = CreateSimpleDataRow(out _, out _);

            Assert.That(row.TryGetValueAs(DefinitivelyNotExistingHeader, typeof(int), out var value), Is.False);
            Assert.That(value, Is.Null);
        }

        [Test]
        public void TryGetValueAsObject_ExistingHeader_AutoConvertsAndReturnCorrectValue() {
            var table = CreateSimpleTable(out var headers, out _);
            var row = table.Skip(1).First();
            for(var i = 0; i < headers.Length; i++) {

                Assert.That(row.TryGetValueAs(headers[i], typeof(int), out var value), Is.True);
                Assert.That(value, Is.EqualTo(i));
            }
        }
        #endregion

        [Test]
        public void DataTable_Values_MatchesInput() {
            var row = CreateSimpleDataRow(out _, out var values);

            CollectionAssert.AreEqual(row.Values, values);
        }

        [Test]
        public void DataTable_Headers_MatchesInput() {
            var row = CreateSimpleDataRow(out var headers, out _);

            CollectionAssert.AreEqual(row.Headers, headers);
        }

        [Test]
        public void DataTable_Rows_MatchesInput() {
            var table = CreateSimpleTable(out var headers, out var values).ToList();

            for(var i = 0; i < values.Length; i++) {
                CollectionAssert.AreEqual(table[i].Headers, headers);
                CollectionAssert.AreEqual(table[i].Values, values[i]);
            }
        }

        private DataRow CreateSimpleDataRow(out string[] headers, out object[] row) {
            var dataRow = CreateSimpleTable(out headers, out _).First();
            row = dataRow.Values;
            return dataRow;
        }
        private DataTable CreateSimpleTable(out string[] headers, out object[][] rows) {
            headers = new[] {
                "field1",
                "field2",
                "field3",
                "field4",
            };

            rows = new[] {
                new object[] {0, 1, 2, 3, 4},
                new object[] {"0", "1", "2", "3", "4"}
            };

            return new DataTable(headers, rows);
        }
    }
}