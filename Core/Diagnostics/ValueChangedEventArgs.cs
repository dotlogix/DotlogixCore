// ==================================================
// Copyright 2016(C) , DotLogix
// File:  ValueChangedEventArgs.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Diagnostics {
    public class ValueChangedEventArgs<T> : EventArgs {
        private T OldValue { get; }
        private T NewValue { get; }

        public ValueChangedEventArgs(T oldValue, T newValue) {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
