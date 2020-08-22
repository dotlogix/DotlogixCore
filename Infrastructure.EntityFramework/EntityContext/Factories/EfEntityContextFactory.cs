// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EfEntityContextFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
#endregion

#region

using System;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Dynamics;
using Microsoft.EntityFrameworkCore;

#endregion

namespace DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext.Factories {
    /// <summary>
    /// An implementation of the <see cref="IEntityContextFactory"/> for entity framework
    /// </summary>
    public class EfEntityContextFactory : IEntityContextFactory {
        private readonly Func<DbContext, IEntityContext> _createEntityContextFunc;

        /// <summary>
        /// The internal <see cref="IDbContextFactory"/>
        /// </summary>
        protected IDbContextFactory DbContextFactory { get; }

        /// <summary>
        /// Create a new instance of <see cref="EfEntityContextFactory"/>
        /// </summary>
        public EfEntityContextFactory(IDbContextFactory contextFactory) {
            DbContextFactory = contextFactory;
            _createEntityContextFunc = (ctx) => new EfEntityContext(ctx);
        }
        
        /// <summary>
        /// Create a new instance of <see cref="EfEntityContextFactory"/>
        /// </summary>
        public EfEntityContextFactory(IDbContextFactory contextFactory, Type entityContextType) {
            DbContextFactory = contextFactory;
            
            if(entityContextType.IsAssignableTo<IEntityContext>() == false)
                throw new ArgumentException($"Type {entityContextType.Name} is not assignable to {nameof(IEntityContext)}");
            
            var ctors = entityContextType.GetConstructors();
            foreach(var ctor in ctors) {
                var parameters = ctor.GetParameters();
                switch(parameters.Length) {
                    case 1: {
                        var p3 = parameters[0].ParameterType;
                        if(p3.IsAssignableTo<DbContext>()) {
                            var dynamicCtor1Arg = ctor.CreateDynamicCtor();
                            _createEntityContextFunc = (v) => (IEntityContext)dynamicCtor1Arg.Invoke(DbContextFactory.Create());
                        }

                        break;
                    }
                    case 0: {
                        var defaultCtor = ctor.CreateDynamicCtor();
                        _createEntityContextFunc = (v) => (IEntityContext)defaultCtor.Invoke();
                        break;
                    }
                    default:
                        continue;
                }
            }
            
            throw new ArgumentException($"Can not find a valid constructor matching one of the available signatures.\n{entityContextType.Name}()\n{entityContextType.Name}(T: DbContext)\n{entityContextType.Name}(T: DbContext, IDictionary<string, object>)");
        }
        
        /// <summary>
        /// Create a new instance of <see cref="EfEntityContextFactory"/>
        /// </summary>
        public EfEntityContextFactory(IDbContextFactory contextFactory, Func<DbContext, IEntityContext> createEntityContextFunc) {
            DbContextFactory = contextFactory;
            _createEntityContextFunc = createEntityContextFunc;
        }
        
        /// <inheritdoc />
        public virtual IEntityContext Create() {
            return _createEntityContextFunc.Invoke(DbContextFactory.Create());
        }
    }
}
