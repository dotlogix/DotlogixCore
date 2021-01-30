// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DynamicRepositoryFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Reflection;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Dynamics;
#endregion

namespace DotLogix.Architecture.Infrastructure.Repositories.Factories {
    /// <summary>
    /// A <see cref="IRepositoryFactory"/> using reflection with il code to create instances
    /// </summary>
    public class DynamicRepositoryFactory : IRepositoryFactory {
        private readonly DynamicCtor _repoCtor;

        /// <summary>
        /// Creates a new instance of <see cref="DynamicRepositoryFactory"/>
        /// </summary>
        public DynamicRepositoryFactory(DynamicCtor repoCtor) {
            _repoCtor = repoCtor;
        }

        /// <summary>
        /// Creates a new repository
        /// </summary>
        public IRepository Create(IEntityContext entityContext) {
            return (IRepository)_repoCtor.Invoke(entityContext);
        }

        /// <summary>
        /// Creates a repository factory using a constructor of the shape .ctor(<see cref="IEntityContext"/>)
        /// </summary>
        public static IRepositoryFactory CreateFor<TRepo>() where TRepo : IRepository {
            return CreateFor(typeof(TRepo));
        }

        /// <summary>
        /// Creates a repository factory using a constructor of the shape .ctor(<see cref="IEntityContext"/>)
        /// </summary>
        public static IRepositoryFactory CreateFor(Type repoType) {
            if(repoType.IsAssignableTo(typeof(IRepository)) == false)
                throw new ArgumentException($"Type {repoType} is not assignable to type {nameof(IRepository)}", nameof(repoType));

            var ctors = repoType.GetConstructors();
            ConstructorInfo repoCtor = null;
            ConstructorInfo defaultCtor = null;
            foreach (var ctor in ctors) {
                var parameters = ctor.GetParameters();
                if(parameters.Length == 1 && parameters[0].ParameterType.IsAssignableTo<IEntityContext>()) {
                    repoCtor = ctor;
                    break;
                }

                if(parameters.Length == 0) {
                    defaultCtor = ctor;
                }
            }

            if(repoCtor == null) {
                if(defaultCtor == null)
                    throw new ArgumentException($"Type {repoType} doesn't have a valid constructor [ {repoType.Name}() | {repoType.Name}(T implements {nameof(IEntityContext)}) ]", nameof(repoType));
                
                repoCtor = defaultCtor;
            }
            var dynamicCtor = repoCtor.CreateDynamicCtor();
            if(dynamicCtor == null)
                throw new ArgumentException($"Can not create dynamic constructor for type {repoType}");
            return new DynamicRepositoryFactory(dynamicCtor);
        }
    }
}
