// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Model.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Architecture.Domain.Models.Base {
    public abstract class Model : InsertOnlyModel, IModel {
        public int Order { get; set; }
    }
}
