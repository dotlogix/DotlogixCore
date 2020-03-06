using System;
using DotLogix.Architecture.Infrastructure.EntityContext;

namespace DotLogix.Architecture.Infrastructure.Attributes {
    /// <summary>
    /// An attribute to describe to apply a function to an <see cref="IEntitySet{TEntity}"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Interface, AllowMultiple = true)]
    public abstract class EntitySetModifierAttribute : Attribute {
        /// <summary>
        /// The execution priority compared to other modifiers
        /// </summary>
        public int Priority { get; }

        /// <summary>
        /// Creates a new instance of <see cref="EntitySetModifierAttribute"/>
        /// </summary>
        protected EntitySetModifierAttribute(int priority) {
            Priority = priority;
        }

        /// <summary>
        /// Creates a modification function which will be applied after a new <see cref="IEntitySet{TEntity}"/> is created
        /// </summary>
        public abstract Func<IEntitySet<TEntity>, IEntitySet<TEntity>> GetModifierFunc<TEntity>() where TEntity : class, new();
    }
}