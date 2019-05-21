// ==================================================
// Copyright 2019(C) , DotLogix
// File:  LambdaProcessingStep.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2019
// LastEdited:  06.02.2019
// ==================================================

#region
using System;
using System.Threading.Tasks;
#endregion

namespace DotLogix.Core.Patterns {
    public class LambdaProcessingStep<TIn, TNext, TResult> : ProcessingStep<TIn, TNext, TResult> {
        private readonly Func<TIn, Func<TNext, Task<TResult>>, Task<TResult>> _callback;

        public LambdaProcessingStep(Func<TIn, Func<TNext, Task<TResult>>, Task<TResult>> callback) {
            _callback = callback;
        }

        public override Task<TResult> InvokeAsync(TIn value) {
            return _callback.Invoke(value, NextAsync);
        }

        public static implicit operator LambdaProcessingStep<TIn, TNext, TResult>(Func<TIn, Func<TNext, Task<TResult>>, Task<TResult>> next) {
            return new LambdaProcessingStep<TIn, TNext, TResult>(next);
        }
    }

    public class LambdaProcessingStep<TIn, TResult> : ProcessingStep<TIn, TResult> {
        private readonly Func<TIn, Task<TResult>> _callback;

        public LambdaProcessingStep(Func<TIn, Task<TResult>> callback) {
            _callback = callback;
        }

        public override Task<TResult> InvokeAsync(TIn value) {
            return _callback.Invoke(value);
        }

        public static implicit operator LambdaProcessingStep<TIn, TResult>(Func<TIn, Task<TResult>> next) {
            return new LambdaProcessingStep<TIn, TResult>(next);
        }
    }
}
