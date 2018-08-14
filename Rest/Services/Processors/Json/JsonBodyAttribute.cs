// ==================================================
// Copyright 2018(C) , DotLogix
// File:  JsonBodyAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.06.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Rest.Services.Processors.Json {
    [AttributeUsage(AttributeTargets.Parameter)]
    public class JsonBodyAttribute : Attribute { }
}
