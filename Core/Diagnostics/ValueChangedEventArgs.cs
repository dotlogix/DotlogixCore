// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ValueChangedEventArgs.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
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
