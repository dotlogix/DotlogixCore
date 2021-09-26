using System;
using System.Collections.Generic;
using DotLogix.Core.Collections;

namespace DotLogix.Infrastructure.EntityFramework.Hooks {
    public class EventHandlerCollection<TArgs> {
        private readonly List<EventHandler<TArgs>> _globalHandlers = new List<EventHandler<TArgs>>();
        private readonly MutableLookup<Type, EventHandler<TArgs>> _filteredHandlers = new MutableLookup<Type, EventHandler<TArgs>>();

        public bool IsEmpty => _globalHandlers.Count == 0 && _filteredHandlers.Count == 0;

        public void AddHandler(Type entityType, EventHandler<TArgs> eventHandler) {
            _filteredHandlers.Add(entityType, eventHandler);
        }
        
        public void AddHandler(EventHandler<TArgs> eventHandler) {
            _globalHandlers.Add(eventHandler);
        }
        
        public bool RemoveHandler(Type entityType, EventHandler<TArgs> eventHandler) {
            return _filteredHandlers.Remove(entityType, eventHandler);
        }
        
        public bool RemoveHandlers(Type entityType) {
            return _filteredHandlers.Remove(entityType);
        }
        
        public bool RemoveHandler(EventHandler<TArgs> eventHandler) {
            return _globalHandlers.Remove(eventHandler);
        }
        
        public void ClearHandlers() {
            _globalHandlers.Clear();
            _filteredHandlers.Clear();
        }
        
        public IReadOnlyCollection<EventHandler<TArgs>> GetHandlers(Type entityType) {
            var entityHandlers = GetEntityHandlers(entityType);
            var globalHandlers = GetGlobalHandlers();

            var hasEntityHandlers = entityHandlers != null;
            var hasGlobalHandlers = globalHandlers != null;
            
            if(hasEntityHandlers != hasGlobalHandlers) {
                return hasEntityHandlers ? entityHandlers : globalHandlers;
            }

            if(hasEntityHandlers == false) {
                return null;
            }
            
            var count = entityHandlers.Count + globalHandlers.Count;
            var list = new List<EventHandler<TArgs>>(count);
            list.AddRange(globalHandlers);
            list.AddRange(entityHandlers);
            return list;
        }
        
        public IReadOnlyCollection<EventHandler<TArgs>> GetGlobalHandlers() {
            return _globalHandlers != null && _globalHandlers.Count > 0 ? _globalHandlers : null;
        }
        public IReadOnlyCollection<EventHandler<TArgs>> GetEntityHandlers(Type entityType) {
            return _filteredHandlers.TryGetValue(entityType, out var values) && values.Count > 0
                       ? values
                       : null;
        }
    }
}