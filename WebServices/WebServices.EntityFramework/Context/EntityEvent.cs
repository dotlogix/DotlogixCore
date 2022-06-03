using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DotLogix.WebServices.EntityFramework.Context {
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
    
    
    
    public class EntityEvent<TArgs> {
        private readonly Event<TArgs> _globalEvent = new();
        private readonly Dictionary<Type, Event<TArgs>> _filteredEvents = new();

        public int Count { get; private set; }

        public void Subscribe(Type entityType, EventHandler<TArgs> eventHandler) {
            if(_filteredEvents.TryGetValue(entityType, out var entityEvent) == false) {
                entityEvent = new Event<TArgs>();
                _filteredEvents.Add(entityType, entityEvent);
            }
            entityEvent.Subscribe(eventHandler);
            Count++;
        }
        public void Subscribe(EventHandler<TArgs> eventHandler) {
            _globalEvent.Subscribe(eventHandler);
            Count++;
        }

        public bool Unsubscribe(Type entityType, EventHandler<TArgs> eventHandler) {
            if(_filteredEvents.TryGetValue(entityType, out var entityEvent) == false)
                return false;
            
            if(entityEvent.Unsubscribe(eventHandler) == false)
                return false;

            Count--;
            return entityEvent.Count > 0 || _filteredEvents.Remove(entityType);
        }
        public bool Unsubscribe(Type entityType) {
            if(_filteredEvents.Remove(entityType) == false)
                return false;
            
            Count--;
            return true;
        }
        public bool Unsubscribe(EventHandler<TArgs> eventHandler) {
            if(_globalEvent.Unsubscribe(eventHandler) == false)
                return false;
            
            Count--;
            return true;
        }
        
        public void Trigger(object sender, TArgs args) {
            _globalEvent.Trigger(sender, args);
        }
        
        public void Trigger(Type entityType, object sender, TArgs args) {
            _globalEvent.Trigger(sender, args);
            if(entityType is not null && _filteredEvents.TryGetValue(entityType, out var entityEvent))
                entityEvent.Trigger(sender, args);
        }
        
        public void Clear() {
            _globalEvent.Clear();
            _filteredEvents.Clear();
            Count = 0;
        }
        
        public IEnumerable<EventHandler<TArgs>> GetHandlers(Type entityType) {
            if(Count == 0)
                return Enumerable.Empty<EventHandler<TArgs>>();
            
            IEnumerable<EventHandler<TArgs>> handlers = null;
            if(_globalEvent.Count > 0)
                handlers = _globalEvent;

            if(_filteredEvents.TryGetValue(entityType, out var entityEvent)) {
                handlers = handlers is null ? entityEvent : handlers.Concat(entityEvent);
            }

            return handlers ?? Enumerable.Empty<EventHandler<TArgs>>();
        }
        
        public IEnumerable<EventHandler<TArgs>> GetGlobalHandlers() {
            return _globalEvent;
        }
        public IEnumerable<EventHandler<TArgs>> GetEntityHandlers(Type entityType) {
            return (IEnumerable<EventHandler<TArgs>>)_filteredEvents.GetValueOrDefault(entityType) ?? Enumerable.Empty<EventHandler<TArgs>>();
        }
    }
}