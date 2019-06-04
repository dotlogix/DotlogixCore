using System;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.EntityContext;

namespace DotLogix.Architecture.Infrastructure.Decorators {
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Interface, AllowMultiple = true)]
    public abstract class EntitySetModifierAttribute : Attribute {
        public int Priority { get; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Attribute"></see> class.</summary>
        protected EntitySetModifierAttribute(int priority) {
            Priority = priority;
        }

        public abstract Func<IEntitySet<TEntity>, IEntitySet<TEntity>> GetModifierFunc<TEntity>() where TEntity : ISimpleEntity;
    }
}