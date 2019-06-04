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
    [InsertOnly]
    public interface IInsertOnlyEntity : ISimpleEntity, IInsertOnly { }

    [Indexed]
    public interface IGuidIndexedEntity : ISimpleEntity { }
}
