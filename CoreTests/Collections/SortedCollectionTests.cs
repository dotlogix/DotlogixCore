using DotLogix.Core.Collections;
using NUnit.Framework;

namespace CoreTests.Collections {
    [TestFixture]
    public class SortedCollectionTests : CollectionTests<int, SortedCollection<int>> {
        private int _currentValue;

        /// <inheritdoc />
        public override void Setup() {
            base.Setup();
            _currentValue = 0;
        }

        /// <inheritdoc />
        protected override SortedCollection<int> CreateCollection() {
            return new SortedCollection<int>();
        }

        /// <inheritdoc />
        protected override int CreateValue() {
            return _currentValue++;
        }
        
        #region Remove
        
        [Test]
        public void EnsureSorted() {
            Collection.Add(1);
            Collection.Add(3);
            Collection.Add(4);
            Collection.Add(5);
            Collection.Add(2);

            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5 }, Collection);
        }

        [Test]
        public void TryRemoveKeyExisting_ItemTryRemoved() {
            var collection = new KeyedCollection<int, int>(i => i) { 1, 2 };
            collection.TryRemoveKey(2);
            CollectionAssert.AreEquivalent(new[] { 1 }, collection);
        }

        #endregion
    }
}
