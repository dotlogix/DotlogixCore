// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  CircularBuffer.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 30.04.2021 01:31
// LastEdited:  26.09.2021 22:27
// ==================================================

#region
using System;
using System.Collections;
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Collections; 

public class CircularBuffer<T> : IReadOnlyCollection<T> {
    private readonly Queue<T> _queue;

    /// <inheritdoc />
    public int Count => _queue.Count;

    public int Capacity { get; }

    public bool AllowOverride { get; }

    public CircularBuffer(int capacity, bool allowOverride = true) {
        AllowOverride = allowOverride;
        Capacity = capacity;
        _queue = new Queue<T>(capacity);
    }

    /// <inheritdoc />
    public IEnumerator<T> GetEnumerator() {
        return _queue.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    public T Peek() {
        return _queue.Peek();
    }

    public void Push(T item) {
        if(_queue.Count >= Capacity) {
            if(AllowOverride) {
                _queue.Dequeue();
            } else {
                throw new InvalidOperationException("Capacity reached");
            }
        }

        _queue.Enqueue(item);
    }

    public T Pop() {
        return _queue.Dequeue();
    }

    public void Clear() {
        _queue.Clear();
    }
}