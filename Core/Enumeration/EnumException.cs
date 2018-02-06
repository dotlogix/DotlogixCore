// ==================================================
// Copyright 2016(C) , DotLogix
// File:  EnumException.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  10.07.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Enumeration {
    public class EnumException : Exception {
        public EnumException() { }
        public EnumException(string message) : base(message) { }
        public EnumException(string message, Exception innerException) : base(message, innerException) { }
    }
}
