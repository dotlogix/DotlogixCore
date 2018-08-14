// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EnumException.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
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
