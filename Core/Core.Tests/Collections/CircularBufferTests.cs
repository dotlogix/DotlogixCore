// ==================================================
// Copyright 2014-2022(C), DotLogix
// File:  CircularBufferTests.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 24.07.2022 16:58
// LastEdited:  24.07.2022 16:58
// ==================================================

using DotLogix.Core.Collections;
using NUnit.Framework;

namespace Core.Tests.Collections; 

[TestFixture]
public class CircularBufferTests {
    [Test]
    public void Peek_Empty_Throws() {
        var buffer = new CircularBuffer<int>(5);
        Assert.That(() => buffer.Peek(), Throws.Exception);
    }
    
    [Test]
    public void Peek_Existing_ReturnsExpected() {
        var buffer = new CircularBuffer<int>(5);
        buffer.Push(1);
        
        Assert.That(buffer.Peek(), Is.EqualTo(1));
        Assert.That(buffer.Count, Is.EqualTo(1));
        Assert.That(buffer.Capacity, Is.EqualTo(5));
    }
    
    [Test]
    public void Dequeue_Empty_Throws() {
        var buffer = new CircularBuffer<int>(5);
        Assert.That(() => buffer.Pop(), Throws.Exception);
    }
    
    [Test]
    public void Dequeue_Existing_ReturnsExpected() {
        var buffer = new CircularBuffer<int>(5);
        buffer.Push(1);
        
        Assert.That(buffer.Pop(), Is.EqualTo(1));
        Assert.That(buffer.Count, Is.EqualTo(0));
        Assert.That(buffer.Capacity, Is.EqualTo(5));
    }
    
    [Test]
    public void Enqueue_Existing_ReturnsExpected() {
        var buffer = new CircularBuffer<int>(5);
        buffer.Push(1);
        buffer.Push(2);
        Assert.That(buffer.Count, Is.EqualTo(2));
        Assert.That(buffer.Capacity, Is.EqualTo(5));
        Assert.That(buffer, Is.EquivalentTo(new[] { 1, 2 }));
    }
    
    [Test]
    public void Enqueue_Wrapping_ReturnsExpected() {
        var buffer = new CircularBuffer<int>(5);
        for(var i = 1; i <= 6; i++) {
            buffer.Push(i);
        }
        Assert.That(buffer.Count, Is.EqualTo(5));
        Assert.That(buffer.Capacity, Is.EqualTo(5));
        Assert.That(buffer, Is.EquivalentTo(new[] { 2, 3, 4, 5, 6 }));
    }
    
    [Test]
    public void Enqueue_NoOverride_Empty_ReturnsExpected() {
        var buffer = new CircularBuffer<int>(5, false);
        buffer.Push(1);
        Assert.That(buffer.Count, Is.EqualTo(1));
        Assert.That(buffer.Capacity, Is.EqualTo(5));
        Assert.That(buffer, Is.EquivalentTo(new[] { 1 }));
    }
    
    [Test]
    public void Enqueue_NoOverride_Existing_ReturnsExpected() {
        var buffer = new CircularBuffer<int>(5, false);
        buffer.Push(1);
        buffer.Push(2);
        Assert.That(buffer.Count, Is.EqualTo(2));
        Assert.That(buffer.Capacity, Is.EqualTo(5));
        Assert.That(buffer, Is.EquivalentTo(new[] { 1, 2 }));
    }
    
    [Test]
    public void Enqueue_NoOverride_Wrapping_Throws() {
        var buffer = new CircularBuffer<int>(5, false);
        for(var i = 1; i <= 5; i++) {
            buffer.Push(i);
        }
        Assert.That(() => buffer.Push(6), Throws.InvalidOperationException);
        Assert.That(buffer.Count, Is.EqualTo(5));
        Assert.That(buffer.Capacity, Is.EqualTo(5));
        Assert.That(buffer, Is.EquivalentTo(new[] { 1, 2, 3, 4, 5 }));
    }
    
    [Test]
    public void Enumerate_KeepsOriginalOrder() {
        var buffer = new CircularBuffer<int>(5);
        for(var i = 1; i <= 8; i++) {
            buffer.Push(i);
        }
        Assert.That(buffer, Is.EquivalentTo(new[] { 4, 5, 6, 7, 8 }));
    }
}