using System;
using DotLogix.Architecture.Common.Options;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Architecture.Infrastructure.Decorators {
	/// <summary>
	/// An attribute to apply an <see cref="IndexedEntitySetDecorator{TEntity}"/> to a <see cref="IEntitySet{TEntity}"/>
	/// </summary>
	public class GuidIndexedAttribute : EntitySetModifierAttribute
	{
		/// <summary>
		/// Creates a new instance of <see cref="GuidIndexedAttribute"/>
		/// </summary>
		public GuidIndexedAttribute(int priority = int.MinValue) : base(priority) { }

		/// <inheritdoc />
		public override Func<IEntitySet<TEntity>, IEntitySet<TEntity>> GetModifierFunc<TEntity>()
		{
			var entitySetType = typeof(IEntitySet<TEntity>);
			var createEntityCacheType = typeof(Func<EntityIndex<TEntity>>);
			var ctor = typeof(IndexedEntitySetDecorator<>)
			           .MakeGenericType(typeof(TEntity))
			           .CreateDynamicCtor(new Type[] { entitySetType, createEntityCacheType });


			return (entitySet) => (IEntitySet<TEntity>)ctor.Invoke(entitySet, (Func<EntityIndex<TEntity>>)OnCreateCache<TEntity>);
		}

		/// <summary>
		/// A callback function to create the underlying entity index
		/// </summary>
		protected virtual EntityIndex<TEntity> OnCreateCache<TEntity>()
		{
			var entityType = typeof(TEntity);
			var dynamicProperty = entityType.GetProperty(nameof(IGuid.Guid))?.CreateDynamicProperty();
			if(dynamicProperty == null)
				throw new ArgumentException("Can not find property Id of type " + entityType.Name);
			return new EntityIndex<TEntity>(dynamicProperty);
		}
	}
	
	/// <summary>
	/// An attribute to apply an <see cref="IndexedEntitySetDecorator{TEntity}"/> to a <see cref="IEntitySet{TEntity}"/>
	/// </summary>
	public class IdentityIndexedAttribute : EntitySetModifierAttribute
	{
		/// <summary>
		/// Creates a new instance of <see cref="GuidIndexedAttribute"/>
		/// </summary>
		public IdentityIndexedAttribute(int priority = int.MinValue) : base(priority) { }

		/// <inheritdoc />
		public override Func<IEntitySet<TEntity>, IEntitySet<TEntity>> GetModifierFunc<TEntity>()
		{
			var entitySetType = typeof(IEntitySet<TEntity>);
			var createEntityCacheType = typeof(Func<EntityIndex<TEntity>>);
			var ctor = typeof(IndexedEntitySetDecorator<>)
			           .MakeGenericType(typeof(TEntity))
			           .CreateDynamicCtor(new Type[] { entitySetType, createEntityCacheType });

			return (entitySet) => (IEntitySet<TEntity>)ctor.Invoke(entitySet, (Func<EntityIndex<TEntity>>)OnCreateCache<TEntity>);
		}

		/// <summary>
		/// A callback function to create the underlying entity index
		/// </summary>
		protected virtual EntityIndex<TEntity> OnCreateCache<TEntity>()
		{
			var entityType = typeof(TEntity);
			var dynamicProperty = entityType.GetProperty(nameof(IIdentity.Id))?.CreateDynamicProperty();
			if(dynamicProperty == null)
				throw new ArgumentException("Can not find property Id of type "+entityType.Name);
			return new EntityIndex<TEntity>(dynamicProperty);
		}
	}
}