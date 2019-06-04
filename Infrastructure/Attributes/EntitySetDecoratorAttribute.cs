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
    public class EntitySetDecoratorAttribute : EntitySetModifierAttribute
    {
        public Type DecoratorType { get; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Attribute"></see> class.</summary>
        public EntitySetDecoratorAttribute(Type decoratorType, int priority) : base(priority)
        {
            DecoratorType = decoratorType;
        }

        public override Func<IEntitySet<TEntity>, IEntitySet<TEntity>> GetModifierFunc<TEntity>()
        {
            var entitySetType = typeof(IEntitySet<TEntity>);
            var decoratorType = DecoratorType.MakeGenericType(typeof(TEntity));

            var ctor = decoratorType.CreateDynamicCtor(new[] { entitySetType });
            return set => (IEntitySet<TEntity>)ctor.Invoke(set);
        }
    }
}