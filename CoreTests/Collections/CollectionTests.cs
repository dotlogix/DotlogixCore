using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace CoreTests.Collections {
    public abstract class CollectionTests<TValue, TCollection> where TCollection : ICollection<TValue> {

        public TCollection Collection { get; private set; }

        [SetUp]
        public virtual void Setup() {
            Collection = CreateCollection();
        }
        
        [TearDown]
        public virtual void Teardown() {
            
        }

        #region Count
        [Test]
        public virtual void Count_Operations() {
            var item1 = CreateValue();
            var item2 = CreateValue();
            var item3 = CreateValue();
            var item4 = CreateValue();
            var item5 = CreateValue();

            Collection.Add(item1);
            Collection.Add(item2);
            Collection.Add(item3);

            Assert.That(Collection.Count, Is.EqualTo(3));

            // Add non existing element
            Collection.Add(item4);
            Assert.That(Collection.Count, Is.EqualTo(4));

            // Add existing element
            Collection.Add(item4);
            Assert.That(Collection.Count, Is.EqualTo(5));

            // Remove existing
            Collection.Remove(item4);
            Assert.That(Collection.Count, Is.EqualTo(4));

            // Remove non existing
            Collection.Remove(item5);
            Assert.That(Collection.Count, Is.EqualTo(4));
        }

        #endregion

        #region Add

        [Test]
        public virtual void Add_NonExisting() {
            var item = CreateValue();
            Collection.Add(item);

            CollectionAssert.AreEquivalent(new []{item}, Collection);
        }

        [Test]
        public virtual void Add_Existing() {
            var item = CreateValue();
            Collection.Add(item);
            Collection.Add(item);

            CollectionAssert.AreEquivalent(new []{item, item }, Collection);
        }

        #endregion

        #region Remove

        [Test]
        public virtual void Remove_NonExisting() {
            var item = CreateValue();
            
            Assert.That(Collection.Remove(item), Is.False);
            Assert.That(Collection, Is.Empty);
        }

        [Test]
        public virtual void Remove_Existing() {
            var item = CreateValue();
            Collection.Add(item);

            Assert.That(Collection.Remove(item), Is.True);
            Assert.That(Collection, Is.Empty);
        }

        #endregion

        #region Contains

        [Test]
        public virtual void Contains_NonExisting() {
            var item = CreateValue();

            Assert.That(Collection.Contains(item), Is.False);
        }

        [Test]
        public virtual void Contains_Existing() {
            var item = CreateValue();
            Collection.Add(item);

            Assert.That(Collection.Contains(item), Is.True);
        }

        #endregion

        #region Clear

        [Test]
        public virtual void Clear_NonExisting() {
            Assert.That(() => Collection.Clear(), Throws.Nothing);
            Assert.That(Collection, Is.Empty);
        }

        [Test]
        public virtual void Clear_Existing() {
            var item = CreateValue();
            Collection.Add(item);

            Assert.That(Collection, Is.Not.Empty);
            Assert.That(() => Collection.Clear(), Throws.Nothing);
            Assert.That(Collection, Is.Empty);
        }

        #endregion


        protected abstract TCollection CreateCollection();
        protected abstract TValue CreateValue();
    }

    public abstract class ListCollectionTests<TValue, TList> : CollectionTests<TValue, TList> where TList : IList<TValue> {
        [Test]
        public override void Count_Operations() {
            base.Count_Operations();

            var count = Collection.Count;
            Collection.RemoveAt(0);
            count--;
            Assert.That(Collection.Count, Is.EqualTo(count));
            
            Assert.That(() => Collection.RemoveAt(-1), Throws.ArgumentException);
            Assert.That(Collection.Count, Is.EqualTo(count));

            Assert.That(() => Collection.RemoveAt(count+1), Throws.ArgumentException);
            Assert.That(Collection.Count, Is.EqualTo(count));
        }

        #region Indexer

        [Test]
        public virtual void Indexer_NonExisting() {
            Assert.That(Collection, Is.Empty);
            Assert.That(() => Collection.RemoveAt(-1), Throws.ArgumentException);
            Assert.That(() => Collection.RemoveAt(0), Throws.ArgumentException);
            Assert.That(() => Collection.RemoveAt(1), Throws.ArgumentException);
        }

        [Test]
        public virtual void Indexer_Existing() {
            var item1 = CreateValue();
            var item2 = CreateValue();
            var item3 = CreateValue();

            Collection.Add(item1);
            Collection.Add(item2);
            Collection.Add(item3);
            
            Assert.That(Collection, Is.Empty);
            Assert.That(Collection[0], Is.EqualTo(item1));
            Assert.That(Collection[1], Is.EqualTo(item2));
            Assert.That(Collection[2], Is.EqualTo(item3));
        }

        #endregion

        #region IndexOf

        [Test]
        public virtual void IndexOf_NonExisting() {
            var item = CreateValue();

            Assert.That(Collection, Is.Empty);
            Assert.That(Collection.IndexOf(item), Is.EqualTo(-1));
        }

        [Test]
        public virtual void IndexOf_Existing() {
            var item1 = CreateValue();
            var item2 = CreateValue();
            var item3 = CreateValue();

            Collection.Add(item1);
            Collection.Add(item2);
            Collection.Add(item3);
            
            Assert.That(Collection, Is.Empty);
            Assert.That(Collection.IndexOf(item1), Is.EqualTo(0));
            Assert.That(Collection.IndexOf(item2), Is.EqualTo(1));
            Assert.That(Collection.IndexOf(item3), Is.EqualTo(2));
        }

        #endregion

        #region RemoveAt

        [Test]
        public virtual void RemoveAt_InvalidLocation() {
            Assert.That(Collection, Is.Empty);
            Assert.That(() => Collection.RemoveAt(-1), Throws.ArgumentException);
            Assert.That(() => Collection.RemoveAt(0), Throws.ArgumentException);
            Assert.That(() => Collection.RemoveAt(1), Throws.ArgumentException);
        }

        [Test]
        public virtual void RemoveAt_Existing() {
            var item1 = CreateValue();
            var item2 = CreateValue();
            var item3 = CreateValue();
            var item4 = CreateValue();
            var item5 = CreateValue();

            Collection.Add(item1);
            Collection.Add(item2);
            Collection.Add(item3);
            Collection.Add(item4);
            Collection.Add(item5);
            Assert.That(Collection.Count, Is.EqualTo(5));

            Collection.RemoveAt(0);
            Assert.That(Collection.Count, Is.EqualTo(4));
            CollectionAssert.AreEquivalent(new []{item2, item3, item4, item5}, Collection);

            Collection.RemoveAt(1);
            Assert.That(Collection.Count, Is.EqualTo(3));
            CollectionAssert.AreEquivalent(new[] { item2, item4, item5 }, Collection);

            Collection.RemoveAt(Collection.Count - 1);
            Assert.That(Collection.Count, Is.EqualTo(2));
            CollectionAssert.AreEquivalent(new[] { item2, item4 }, Collection);
        }

        #endregion
    }
}
