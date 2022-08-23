using System.Collections.Generic;
using DotLogix.Core.Collections;
using NUnit.Framework;

namespace Core.Tests.Collections; 

[TestFixture]
public class KeyedCollectionTests : CollectionTests<int, KeyedCollection<int, int>> {
    private int _currentValue;

    /// <inheritdoc />
    public override void Setup() {
        base.Setup();
        _currentValue = 0;
    }

    /// <inheritdoc />
    protected override KeyedCollection<int, int> CreateCollection() {
        return new KeyedCollection<int, int>(i => i);
    }

    /// <inheritdoc />
    protected override int CreateValue() {
        return _currentValue++;
    }

    [Test]
    public override void Count_Operations() {
        var collection = new KeyedCollection<int, int>(i => i) { 1, 2, 3 };
        Assert.That(collection.Count, Is.EqualTo(3));

        // Add non existing element
        collection.Add(4);
        Assert.That(collection.Count, Is.EqualTo(4));

        // Add existing existing element
        Assert.That(() => collection.Add(4), Throws.Exception);
        Assert.That(collection.Count, Is.EqualTo(4));

        // Add existing element
        collection.TryAdd(4);
        Assert.That(collection.Count, Is.EqualTo(4));

        // Remove existing
        collection.Remove(4);
        Assert.That(collection.Count, Is.EqualTo(3));

        // Remove non existing
        collection.Remove(4);
        Assert.That(collection.Count, Is.EqualTo(3));
    }

    [Test]
    public void Keys_EqualsExpected() {
        var collection = new KeyedCollection<int, int>(i => i) { 1, 2, 3 };
        CollectionAssert.AreEquivalent(new []{1,2,3}, collection.Keys);
    }

    [Test]
    public void Pairs_EqualsExpected() {
        var collection = new KeyedCollection<int, int>(i => i) { 1, 2, 3 };
        var dictionary = new Dictionary<int, int>{ {1,1}, {2,2}, {3,3}};

        CollectionAssert.AreEquivalent(dictionary, collection.Pairs);
    }

    #region Get
    [Test]
    public void GetExisting_ReturnsTrue() {
        var collection = new KeyedCollection<int, int>(i => i) { 1 };
        Assert.That(collection.TryGetValue(1, out _), Is.True);
    }

    [Test]
    public void GetExisting_ReturnsItem() {
        var collection = new KeyedCollection<int, int>(i => i) { 1 };
        collection.TryGetValue(1, out var v);
        Assert.That(v, Is.EqualTo(1));
    }

    [Test]
    public void GetNonExisting_ReturnsFalse() {
        var collection = new KeyedCollection<int, int>(i => i);
        Assert.That(collection.TryGetValue(1, out _), Is.False);
    } 

    [Test]
    public void GetExisting_ReturnsDefaultValue() {
        var collection = new KeyedCollection<int, int>(i => i);
        collection.TryGetValue(1, out var v);
        Assert.That(v, Is.EqualTo(default(int)));
    }

    #endregion

    #region GetOrAdd
    [Test]
    public void GetOrAddNew_ItemsArePresent() {
        var newValue = (1, 2);

        var collection = new KeyedCollection<int, (int key, int value)>(p => p.key);
        Assert.That(collection.GetOrAdd(newValue), Is.EqualTo(newValue));
        CollectionAssert.AreEquivalent(new[] { newValue }, collection);
    }

    [Test]
    public void GetOrAddExisting_ReturnsExisting() {
        var existingValue = (1, 2);
        var newValue = (1, 3);

        var collection = new KeyedCollection<int, (int key, int value)>(p => p.key) {existingValue};
        Assert.That(collection.GetOrAdd(newValue), Is.EqualTo(existingValue));
    }

    [Test]
    public void GetOrAddExisting_NoChanges() {
        var existingValue = (1, 2);
        var newValue = (1, 3);

        var collection = new KeyedCollection<int, (int key, int value)>(p => p.key) {existingValue};

        collection.GetOrAdd(newValue);
        CollectionAssert.AreEquivalent(new[] { existingValue }, collection);
    }

    #endregion

    #region Add

    [Test]
    public override void Add_Existing() {
        Collection.Add(1);
        Assert.That(() => Collection.Add(1), Throws.Exception);
        CollectionAssert.AreEquivalent(new[] { 1 }, Collection);
    }

    [Test]
    public void TryAddNew_ReturnsTrue() {
        var collection = new KeyedCollection<int, int>(i => i);
        Assert.That(collection.TryAdd(1), Is.True);
    }

    [Test]
    public void TryAddNew_ItemAdded() {
        var collection = new KeyedCollection<int, int>(i => i);
        collection.TryAdd(1);
        CollectionAssert.AreEquivalent(new[]{ 1 }, collection);
    }

    [Test]
    public void TryAddExisting_ReturnsFalse() {
        var collection = new KeyedCollection<int, int>(i => i) { 1 };
        Assert.That(collection.TryAdd(1), Is.False);
    }

    [Test]
    public void TryAddExisting_NoChanges() {
        var collection = new KeyedCollection<int, int>(i => i) { 1 };
        collection.TryAdd(1);
        CollectionAssert.AreEquivalent(new[]{ 1 }, collection);
    }

    #endregion

    #region Remove
        
    [Test]
    public void TryRemoveKeyExisting_ReturnsTrue() {
        var collection = new KeyedCollection<int, int>(i => i) { 1, 2 };

        Assert.That(collection.TryRemoveKey(2), Is.True);
        CollectionAssert.AreEquivalent(new[] { 1 }, collection);
    }
    [Test]
    public void TryRemoveKeyExisting_ItemTryRemoved() {
        var collection = new KeyedCollection<int, int>(i => i) { 1, 2 };
        collection.TryRemoveKey(2);
        CollectionAssert.AreEquivalent(new[] { 1 }, collection);
    }

    [Test]
    public void TryRemoveKeyNonExisting_ReturnsFalse() {
        var collection = new KeyedCollection<int, int>(i => i) { 1, 2 };
        Assert.That(collection.TryRemoveKey(3), Is.False);
    }

    [Test]
    public void TryRemoveKeyNonExisting_NoChanges() {
        var collection = new KeyedCollection<int, int>(i => i) { 1, 2 };
        collection.TryRemoveKey(3);
        CollectionAssert.AreEquivalent(new[] { 1, 2 }, collection);
    }

    #endregion
}