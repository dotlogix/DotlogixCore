using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotLogix.Core;
using DotLogix.Core.Caching;
using NUnit.Framework;
using DotLogix.Core.Extensions;
using NuGet.Frameworks;

namespace CoreTests {
    [TestFixture]
    public class CacheTests {
        private Cache<string, string> _cache;

        [SetUp]
        public void Setup() {
            _cache = new Cache<string, string>(TimeSpan.FromSeconds(15));
        }


        #region Cache Store
        [Test]
        public void Cache_Store_NonExisting() {
            var expectedItem = CreateSimpleItem("test", "testValue");
            expectedItem.Children.Add("child1");
            expectedItem.AssociatedValues.Add("prop1", "propValue1");


            var previousItemCount = _cache.Count;
            var storedItem = StoreItem(_cache, expectedItem);
            var currentItemCount = _cache.Count;

            Assert.That(currentItemCount, Is.EqualTo(previousItemCount + 1));
            

            AssertItemsAreEqual(storedItem, expectedItem);
        }

        [Test]
        public void Cache_Store_Existing_NoPreserve() {
            var expectedItem = CreateSimpleItem("test", "testValue");

            var firstItem = StoreItem(_cache, expectedItem); // store previous item with same key
            firstItem.Children.Add("child1");
            firstItem.AssociatedValues.Add("prop1", "propValue1");


            expectedItem = CreateSimpleItem("test", "testValue2");

            var previousItemCount = _cache.Count;
            var secondItem = StoreItem(_cache, expectedItem, false); // store with preserve context false
            var currentItemCount = _cache.Count;
            Assert.That(currentItemCount, Is.EqualTo(previousItemCount));

            AssertItemsAreEqual(secondItem, expectedItem);
        }

        [Test]
        public void Cache_Store_Existing_Preserve() {
            var expectedItem = CreateSimpleItem("test", "testValue");
            expectedItem.Children.Add("child1");
            expectedItem.AssociatedValues.Add("prop1", "propValue1");

            var firstItem = StoreItem(_cache, expectedItem); // store previous item with same key
            AssertItemsAreEqual(firstItem, expectedItem);


            var secondExpectedItem = CreateSimpleItem("test", "testValue2");
            // Adjust to match the new stored item, children & props remain as above
            expectedItem.Value = "testValue2";
            expectedItem.Policy = secondExpectedItem.Policy;

            var previousItemCount = _cache.Count;
            var secondItem = StoreItem(_cache, secondExpectedItem, true); // store with preserve context true
            var currentItemCount = _cache.Count;
            Assert.That(currentItemCount, Is.EqualTo(previousItemCount));

            AssertItemsAreEqual(secondItem, expectedItem);
        }

        #endregion

        #region Cache Retrieve

        [Test]
        public void Cache_Retrieve_NonExisting_ReturnsNull() {
            CacheItem<string, string> expectedItem = null;
            Assert.DoesNotThrow(() => { expectedItem = _cache.RetrieveItem("nonExistingKey"); });
            Assert.That(expectedItem, Is.Null);
        }

        [Test]
        public void Cache_Retrieve_Existing() {
            var expectedItem = CreateSimpleItem("test", "testValue");
            expectedItem.Children.Add("child1");
            expectedItem.AssociatedValues.Add("prop1", "propValue1");

            var storedItem = StoreItem(_cache, expectedItem);
            var retrievedItem = _cache.RetrieveItem(expectedItem.Key);

            AssertItemsAreEqual(retrievedItem, expectedItem);
            AssertItemsAreEqual(retrievedItem, storedItem);
        }

        #endregion

        #region Cache Discard

        [Test]
        public void Cache_Discard_NonExisting_ReturnsFalse() {
            Assert.That(_cache.Discard("nonExistingKey"), Is.False);
        }

        [Test]
        public void Cache_Discard_Existing() {
            var childItem = CreateSimpleItem("child1", "testValue");
            var parentItem = CreateSimpleItem("test", "testValue");
            parentItem.Children.Add("child1");

            StoreItem(_cache, parentItem);
            StoreItem(_cache, childItem);

            Assert.That(_cache.IsAlive(parentItem.Key), Is.True);
            Assert.That(_cache.IsAlive(childItem.Key), Is.True);
            Assert.That(_cache.Discard(parentItem.Key), Is.True);


            Assert.That(_cache.IsAlive(parentItem.Key), Is.False);
            Assert.That(_cache.IsAlive(childItem.Key), Is.False);
        }

        [Test]
        public void Cache_DiscardChildren_NonExisting_ReturnsFalse() {
            Assert.That(_cache.DiscardChildren("nonExistingKey"), Is.False);
        }

        [Test]
        public void Cache_DiscardChildren_Existing() {
            var childItem = CreateSimpleItem("child1", "testValue");
            var parentItem = CreateSimpleItem("test", "testValue");
            parentItem.Children.Add("child1");

            parentItem = StoreItem(_cache, parentItem);
            childItem = StoreItem(_cache, childItem);

            Assert.That(_cache.IsAlive(parentItem.Key), Is.True);
            Assert.That(_cache.IsAlive(childItem.Key), Is.True);
            Assert.That(_cache.DiscardChildren(parentItem.Key), Is.True);

            Assert.That(_cache.IsAlive(parentItem.Key), Is.True);
            Assert.That(parentItem.HasChildren, Is.False);
            Assert.That(_cache.IsAlive(childItem.Key), Is.False);
        }


        [Test]
        public void Cache_DiscardEvent_Existing() {
            var childItem = CreateSimpleItem("child1", "testValue");
            var parentItem = CreateSimpleItem("test", "testValue");
            parentItem.Children.Add("child1");

            parentItem = StoreItem(_cache, parentItem);
            childItem = StoreItem(_cache, childItem);

            CacheItemsDiscardedEventArgs<string, string> args = null;
            _cache.ItemsDiscarded += (s, a) => args = a;


            Assert.That(_cache.IsAlive(parentItem.Key), Is.True);
            Assert.That(_cache.IsAlive(childItem.Key), Is.True);
            Assert.That(_cache.Discard(parentItem.Key), Is.True);

            Assert.That(args.Reason, Is.EqualTo(CacheItemDiscardReason.Discarded));
            Assert.That(args.DiscardedItems.Count, Is.EqualTo(2));

            var parentItemArgs = args.DiscardedItems.FirstOrDefault(i => i.Item.Key == parentItem.Key);
            var childItemArgs = args.DiscardedItems.FirstOrDefault(i => i.Item.Key == childItem.Key);
            
            Assert.That(parentItemArgs, Is.Not.Null);
            Assert.That(childItemArgs, Is.Not.Null);

            Assert.That(parentItemArgs.Reason, Is.EqualTo(CacheItemDiscardReason.Discarded));
            Assert.That(parentItemArgs.Source, Is.EqualTo(CacheItemDiscardSource.Self));
            Assert.That(parentItemArgs.Ancestors, Is.Empty);
            Assert.That(parentItemArgs.Dependencies.Count, Is.EqualTo(1));
            Assert.That(parentItemArgs.Dependencies, Contains.Item(childItemArgs));

            Assert.That(childItemArgs.Reason, Is.EqualTo(CacheItemDiscardReason.Discarded));
            Assert.That(childItemArgs.Source, Is.EqualTo(CacheItemDiscardSource.Ancestor));
            Assert.That(childItemArgs.Ancestors.Count, Is.EqualTo(1));
            Assert.That(childItemArgs.Ancestors, Contains.Item(parentItemArgs));
            Assert.That(childItemArgs.Dependencies, Is.Empty);
        }

        #endregion

        #region CacheItem Basics

        [Test]
        public void CacheItem_Properties() {
            var firstItem = CreateSimpleItem("key", "value");
            Assert.That(firstItem.HasAssociatedValues, Is.False);

            var propPair = new KeyValuePair<object, object>("prop1", "propValue1");

            firstItem.AssociatedValues.Add(propPair.Key, propPair.Value);
            Assert.That(firstItem.HasAssociatedValues, Is.True);
            Assert.That(firstItem.AssociatedValues, Is.Not.Null);
            Assert.That(firstItem.AssociatedValues.TryGetValue(propPair.Key, out var av), Is.True);
            Assert.That(av, Is.EqualTo(propPair.Value));

            firstItem.AssociatedValues.Clear();
            Assert.That(firstItem.HasAssociatedValues, Is.False);
        }

        [Test]
        public void CacheItem_Children() {
            var firstItem = CreateSimpleItem("key", "value");
            Assert.That(firstItem.HasChildren, Is.False);

            var canAdd = firstItem.Children.Add("child1");
            Assert.That(canAdd, Is.True);
            Assert.That(firstItem.HasChildren, Is.True);
            Assert.That(firstItem.Children, Is.Not.Null);
            Assert.That(firstItem.Children, Contains.Item("child1"));

            firstItem.Children.Clear();
            Assert.That(firstItem.HasChildren, Is.False);
        }

        #endregion

        #region Helper

        private static CacheItem<TKey, TValue> CreateSimpleItem<TKey, TValue>(TKey key, TValue value = default, ICachePolicy policy = null) {
            return new CacheItem<TKey, TValue>(key, value, policy);
        }

        private static CacheItem<TKey, TValue> StoreItem<TKey, TValue>(Cache<TKey, TValue> cache, CacheItem<TKey, TValue> item, bool preserveContext = true) {
            var storedItem = cache.Store(item.Key, item.Value, item.Policy, preserveContext);
            if(item.HasChildren) {
                if(storedItem.HasChildren)
                    throw new NotSupportedException("Merge of children is not intended");

                storedItem.Children.UnionWith(item.Children);
            }

            if(item.HasAssociatedValues) {
                if(storedItem.HasAssociatedValues)
                    throw new NotSupportedException("Merge of properties is not intended");

                storedItem.AssociatedValues.Union(item.AssociatedValues);
            }

            return storedItem;
        }

        private static void AssertItemsAreEqual<TKey, TValue>(CacheItem<TKey, TValue> actual, CacheItem<TKey, TValue> expected) {
            Assert.That(actual.Key, Is.EqualTo(expected.Key));
            Assert.That(actual.Value, Is.EqualTo(expected.Value));
            Assert.That(actual.Policy, Is.EqualTo(expected.Policy));
            Assert.That(actual.HasChildren, Is.EqualTo(expected.HasChildren));
            Assert.That(actual.HasAssociatedValues, Is.EqualTo(expected.HasAssociatedValues));

            if(actual.HasChildren) {
                Assert.That(actual.Children, Is.EqualTo(expected.Children));
            }

            if(actual.HasAssociatedValues) {
                Assert.That(actual.AssociatedValues, Is.EqualTo(expected.AssociatedValues));
            }
        }

        #endregion
    }
}
