// ==================================================
// Copyright 2019(C) , DotLogix
// File:  ProcessingStep.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2019
// LastEdited:  06.02.2019
// ==================================================

#region
using System;
using System.Threading.Tasks;
#endregion

namespace DotLogix.Core.Patterns {
    public abstract class ProcessingStep<TIn, TResult> {
        public abstract Task<TResult> InvokeAsync(TIn value);
    }

    public abstract class ProcessingStep<TIn, TNext, TResult> : ProcessingStep<TIn, TResult> {
        private ProcessingStep<TNext, TResult> _nextStep;

        protected virtual Task<TResult> NextAsync(TNext value) {
            if(_nextStep == null)
                throw new InvalidOperationException("This step must return a value or define a next step");
            return _nextStep.InvokeAsync(value);
        }

        public virtual ProcessingStep<TIn, TResult> EndWith(ProcessingStep<TNext, TResult> next) {
            _nextStep = next;
            return this;
        }

        public virtual ProcessingStep<TIn, T, TResult> ContinueWith<T>(ProcessingStep<TNext, T, TResult> next) {
            _nextStep = next;
            return new WrappedProcessingStep<TIn, TNext, T, TResult>(this, next);
        }

        public ProcessingStep<TIn, TResult> EndWith(Func<TNext, Task<TResult>> next) {
            var nextStep = new LambdaProcessingStep<TNext, TResult>(next);
            return EndWith(nextStep);
        }

        public ProcessingStep<TIn, TNext, TResult> ContinueWith(Func<TNext, Func<TNext, Task<TResult>>, Task<TResult>> next) {
            return ContinueWith<TNext>(next);
        }

        public ProcessingStep<TIn, T, TResult> ContinueWith<T>(Func<TNext, Func<T, Task<TResult>>, Task<TResult>> next) {
            var nextStep = new LambdaProcessingStep<TNext, T, TResult>(next);
            return ContinueWith(nextStep);
        }
    }
}
