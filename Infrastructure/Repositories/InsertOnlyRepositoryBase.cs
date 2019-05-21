// ==================================================
// Copyright 2018(C) , DotLogix
// File:  InsertOnlyRepositoryBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  04.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Architecture.Common.Entities;
using DotLogix.Architecture.Infrastructure.EntityContext;
#endregion

namespace DotLogix.Architecture.Infrastructure.Repositories {
    public class InsertOnlyRepositoryBase<TEntity> : RepositoryBase<TEntity>, IInsertOnlyRepository<TEntity> where TEntity : class, IInsertOnlyEntity, new() {
        public InsertOnlyRepositoryBase(IEntitySetProvider entitySetProvider, bool allowCaching = true) : base(entitySetProvider, allowCaching) { }

        protected override IEntitySet<TEntity> OnModifyEntitySet(IEntitySet<TEntity> set) {
            set = new InsertOnlyEntitySetDecorator<TEntity>(set);
            if(AllowCaching)
                set = new GuidIndexedEntitySetDecorator<TEntity>(set, OnCreateCache());
            return set;
        }
    }
}
