// ==================================================
// Copyright 2018(C) , DotLogix
// File:  MeasurementValue.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Diagnostics {
    public class MeasurementValue<TValue> {
        private uint _historySize;

        public uint HistorySize {
            get => _historySize;
            set {
                var newHist = new TValue[value];
                for(var i = 0; (i < _historySize) && (i < value); i++)
                    newHist[i] = History[i];
                History = newHist;
                _historySize = value;
            }
        }

        public TValue[] History { get; private set; }

        public TValue Current { get; private set; }

        public TValue this[int idx] {
            get {
                if(idx >= HistorySize)
                    throw new ArgumentOutOfRangeException(nameof(idx), "Index is not a valid HistoryIndex");
                if(idx == 0)
                    return Current;
                return History[idx - 1];
            }
        }

        public MeasurementValue() {
            Reset();
        }

        public event EventHandler<ValueChangedEventArgs<TValue>> ValueChanged;

        public void UpdateValue(TValue newValue) {
            for(var i = (int)_historySize - 1; i > 0; i--)
                History[i] = History[i - 1];
            History[0] = Current;
            Current = newValue;
        }

        public void Clear() {
            History = new TValue[_historySize];
            Current = default(TValue);
        }

        public void Reset() {
            _historySize = 10;
            Clear();
        }

        protected virtual void OnValueChanged(TValue oldValue, TValue newValue) {
            ValueChanged?.Invoke(this, new ValueChangedEventArgs<TValue>(oldValue, newValue));
        }
    }
}
