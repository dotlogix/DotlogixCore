// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IInsertOnlyEntity.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Architecture.Common.Options;
using DotLogix.Architecture.Infrastructure.Decorators;
#endregion

namespace DotLogix.Architecture.Infrastructure.Entities {
    /// <summary>
    /// An interface combining <see cref="ISimpleEntity"/> and <see cref="IInsertOnly"/> interface
    /// </summary>
    [InsertOnly]
    public interface IInsertOnlyEntity : ISimpleEntity, IInsertOnly { }

    /// <summary>
    /// An interface to activate second level caching for <see cref="ISimpleEntity"/>
    /// </summary>
    [Indexed]
    public interface IGuidIndexedEntity : ISimpleEntity { }
}
