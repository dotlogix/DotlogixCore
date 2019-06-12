// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ISimpleEntity.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Architecture.Common.Options;
#endregion

namespace DotLogix.Architecture.Infrastructure.Entities {
    /// <summary>
    /// An interface combining <see cref="IIdentity"/> and <see cref="IGuid"/> interface
    /// </summary>
    public interface ISimpleEntity : IIdentity, IGuid { }
}
