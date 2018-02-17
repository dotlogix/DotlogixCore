// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IInsertOnlyEntity.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using DotLogix.Architecture.Infrastructure.Entities.Options;
#endregion

namespace DotLogix.Architecture.Infrastructure.Entities {
    public interface IInsertOnlyEntity : ISimpleEntity, IInsertOnly { }
}
