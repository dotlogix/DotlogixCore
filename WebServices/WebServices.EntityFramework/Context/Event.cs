using System;
using System.Collections;
using System.Collections.Generic;

namespace DotLogix.WebServices.EntityFramework.Context; 

public class Event<TArgs> : IReadOnlyCollection<EventHandler<TArgs>> {
    private readonly List<EventHandler<TArgs>> _handlers = new();
        
    public event EventHandler<TArgs> Triggered {
        add => Subscribe(value);
        remove => Unsubscribe(value);
    }
        
    public int Count => _handlers.Count;
        
    public void Subscribe(EventHandler<TArgs> eventHandler) {
        _handlers.Add(eventHandler);
    }
        
    public bool Unsubscribe(EventHandler<TArgs> eventHandler) {
        return _handlers.Remove(eventHandler);
    }

    public void Trigger(object sender, TArgs args) {
        foreach(var handler in _handlers) {
            handler.Invoke(sender, args);
        }
    }
        
    public void Clear() {
        _handlers.Clear();
    }


    public IEnumerator<EventHandler<TArgs>> GetEnumerator() {
        return _handlers.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}