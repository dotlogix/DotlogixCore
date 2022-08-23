// ==================================================
// Copyright 2014-2022(C), DotLogix
// File:  ModelBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 25.06.2022 18:08
// LastEdited:  25.06.2022 18:08
// ==================================================

using System;
using DotLogix.Common.Features;

namespace DotLogix.WebServices.Adapters.Models; 

public class ResponseBase : IGuid {
    public Guid Guid { get; set; }
}