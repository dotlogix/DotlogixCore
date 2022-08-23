using System.Collections.Generic;
using NUnit.Framework;

namespace Core.Tests.Collections; 

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