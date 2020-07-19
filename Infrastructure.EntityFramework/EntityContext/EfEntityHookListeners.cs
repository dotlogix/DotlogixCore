using System;
using DotLogix.Core.Collections;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext {
    public class EfEntityHookListeners {
        public EfEntityHookListeners(MutableLookup<Type, EventHandler<EntityTrackedEventArgs>> entityTracked, MutableLookup<Type, EventHandler<EntityStateChangedEventArgs>> entityStateChanged, MutableLookup<Type, EventHandler<EntityCommitEventArgs>> entityCommit, MutableLookup<Type, EventHandler<EntityCommitEventArgs>> entityCommitted) {
            EntityTracked = entityTracked;
            EntityStateChanged = entityStateChanged;
            EntityCommit = entityCommit;
            EntityCommitted = entityCommitted;
        }
        public MutableLookup<Type, EventHandler<EntityTrackedEventArgs>> EntityTracked { get; }
        public MutableLookup<Type, EventHandler<EntityStateChangedEventArgs>> EntityStateChanged { get; }
        public MutableLookup<Type, EventHandler<EntityCommitEventArgs>> EntityCommit { get; }
        public MutableLookup<Type, EventHandler<EntityCommitEventArgs>> EntityCommitted { get; }
    }
}