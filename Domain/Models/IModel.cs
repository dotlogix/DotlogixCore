// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IEntity.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Architecture.Common.Options;
#endregion

namespace DotLogix.Architecture.Domain.Models {
    /// <summary>
    /// An interface combining <see cref="IInsertOnlyModel"/> and <see cref="IOrdered"/> interface
    /// </summary>
    public interface IModel : IInsertOnlyModel, IOrdered { }
}
