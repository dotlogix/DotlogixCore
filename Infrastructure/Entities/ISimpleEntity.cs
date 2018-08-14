// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ISimpleEntity.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Architecture.Infrastructure.Entities.Options;
#endregion

namespace DotLogix.Architecture.Infrastructure.Entities {
    public interface ISimpleEntity : IIdentity, IGuid { }
}
