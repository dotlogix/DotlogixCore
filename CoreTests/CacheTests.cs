using System;
using System.Collections.Generic;
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

        public void Cache_Retrieve_NonExisting_ReturnsNull() {
            CacheItem<string, string> expectedItem = null;
            Assert.DoesNotThrow(() => { expectedItem = _cache.RetrieveItem("nonExistingKey"); });
            Assert.That(expectedItem, Is.Null);
        }

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
        
        #region CacheItem Basics

        public void CacheItem_Properties() {
            var firstItem = CreateSimpleItem("key", "value");
            Assert.That(firstItem.HasAssociatedValues, Is.False);

            var propPair = new KeyValuePair<object, object>("prop1", "propValue1");

            firstItem.AssociatedValues.Add(propPair.Key, propPair.Value);
            Assert.That(firstItem.HasAssociatedValues, Is.True);
            Assert.That(firstItem.AssociatedValues, Is.Not.Null);
            Assert.That(firstItem.AssociatedValues, Contains.Value(propPair));

            firstItem.AssociatedValues.Clear();
            Assert.That(firstItem.HasAssociatedValues, Is.False);
        }

        public void CacheItem_Children() {
            var firstItem = CreateSimpleItem("key", "value");
            Assert.That(firstItem.HasChildren, Is.False);

            var canAdd = firstItem.Children.Add("child1");
            Assert.That(canAdd, Is.True);
            Assert.That(firstItem.HasChildren, Is.True);
            Assert.That(firstItem.Children, Is.Not.Null);
            Assert.That(firstItem.Children, Contains.Value("child1"));

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

            return item;
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
