// ==================================================
// Copyright 2019(C) , DotLogix
// File:  EntitySetDecoratorAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  ..
// LastEdited:  02.06.2019
// ==================================================

using System;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Architecture.Infrastructure.Decorators {
    /// <summary>
    /// An attribute to describe an <see cref="EntitySetDecoratorBase{TEntity}"/> which should be applied to this class
    /// </summary>
    public class EntitySetDecoratorAttribute : EntitySetModifierAttribute
    {
        /// <summary>
        /// The decorator type
        /// </summary>
        public Type DecoratorType { get; }

        /// <summary>
        /// Creates a new instance of <see cref="EntitySetDecoratorBase{TEntity}"/>
        /// </summary>
        public EntitySetDecoratorAttribute(Type decoratorType, int priority) : base(priority)
        {
            DecoratorType = decoratorType;
        }

        /// <inheritdoc />
        public override Func<IEntitySet<TEntity>, IEntitySet<TEntity>> GetModifierFunc<TEntity>()
        {
            var entitySetType = typeof(IEntitySet<TEntity>);
            var decoratorType = DecoratorType.MakeGenericType(typeof(TEntity));

            var ctor = decoratorType.CreateDynamicCtor(new[] { entitySetType });
            return set => (IEntitySet<TEntity>)ctor.Invoke(set);
        }
    }
}