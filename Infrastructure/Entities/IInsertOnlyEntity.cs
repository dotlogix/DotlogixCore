// ==================================================
// Copyright 2016(C) , DotLogix
// File:  IInsertOnlyEntity.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  24.08.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using DotLogix.Architecture.Infrastructure.Entities.Options;
#endregion

namespace DotLogix.Architecture.Infrastructure.Entities {
    public interface IInsertOnlyEntity : ISimpleEntity, IInsertOnly { }
}
