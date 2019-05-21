﻿// ==================================================
// Copyright 2019(C) , DotLogix
// File:  WrappedProcessingStep.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2019
// LastEdited:  06.02.2019
// ==================================================

#region
using System.Threading.Tasks;
#endregion

namespace DotLogix.Core.Patterns {
    public class WrappedProcessingStep<TIn, TInTail, TNext, TResult> : ProcessingStep<TIn, TNext, TResult> {
        private readonly ProcessingStep<TIn, TResult> _head;
        private readonly ProcessingStep<TInTail, TNext, TResult> _tail;

        public WrappedProcessingStep(ProcessingStep<TIn, TResult> head, ProcessingStep<TInTail, TNext, TResult> tail) {
            _head = head;
            _tail = tail;
        }

        public override Task<TResult> InvokeAsync(TIn value) {
            return _head.InvokeAsync(value);
        }

        public override ProcessingStep<TIn, TResult> EndWith(ProcessingStep<TNext, TResult> next) {
            _tail.EndWith(next);
            return _head;
        }

        public override ProcessingStep<TIn, T, TResult> ContinueWith<T>(ProcessingStep<TNext, T, TResult> next) {
            _tail.ContinueWith(next);
            return new WrappedProcessingStep<TIn, TNext, T, TResult>(_head, next);
        }
    }
}
