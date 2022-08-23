// ==================================================
// Copyright 2014-2022(C), DotLogix
// File:  EntityBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 25.06.2022 03:32
// LastEdited:  25.06.2022 03:32
// ==================================================

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DotLogix.Common.Features;

namespace DotLogix.WebServices.EntityFramework.Entities; 

public abstract class EntityBase : IIdentity, IGuid
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Key]
    public Guid Guid { get; set; }
}