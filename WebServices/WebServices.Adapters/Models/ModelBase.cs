// ==================================================
// Copyright 2018(C) , DotLogix
// File:  SimpleModel.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Common.Features;
#endregion

namespace DotLogix.WebServices.Adapters.Models {
    /// <summary>
    /// A base class for models
    /// </summary>
    public abstract class ModelBase : IGuid {
        /// <inheritdoc />
        public Guid Guid { get; set; }
    }
}
