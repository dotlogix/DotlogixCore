// ==================================================
// Copyright 2018(C) , DotLogix
// File:  SimpleModel.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Architecture.Domain.Models.Base {
    public abstract class SimpleModel : ISimpleModel {
        public Guid Guid { get; set; }
    }
}
