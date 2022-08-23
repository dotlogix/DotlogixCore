using System.Collections.Generic;
using NUnit.Framework;

namespace Core.Tests.Collections; 

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