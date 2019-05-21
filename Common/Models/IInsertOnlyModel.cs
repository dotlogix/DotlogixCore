// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IInsertOnlyEntity.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Architecture.Common.Options;
#endregion

namespace DotLogix.Architecture.Common.Models {
    public interface IInsertOnlyModel : ISimpleModel, IInsertOnly { }
}
