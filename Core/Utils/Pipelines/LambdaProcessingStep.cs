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

namespace DotLogix.Core.Utils.Pipelines {
    /// <summary>
    /// A pipeline step calling another method
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TNext"></typeparam>
    public class LambdaProcessingStep<TIn, TNext, TResult> : ProcessingStep<TIn, TNext, TResult> {
        private readonly Func<TIn, Func<TNext, Task<TResult>>, Task<TResult>> _callback;

        /// <summary>
        /// Creates a new instance of <see cref="LambdaProcessingStep{TIn,TNext,TResult}"/>
        /// </summary>
        /// <param name="callback"></param>
        public LambdaProcessingStep(Func<TIn, Func<TNext, Task<TResult>>, Task<TResult>> callback) {
            _callback = callback;
        }

        /// <inheritdoc />
        public override Task<TResult> InvokeAsync(TIn value) {
            return _callback.Invoke(value, NextAsync);
        }

        /// <summary>
        /// Implicitly converts a method to a processing step
        /// </summary>
        /// <param name="next"></param>
        /// <returns></returns>
        public static implicit operator LambdaProcessingStep<TIn, TNext, TResult>(Func<TIn, Func<TNext, Task<TResult>>, Task<TResult>> next) {
            return new LambdaProcessingStep<TIn, TNext, TResult>(next);
        }
    }

    /// <summary>
    /// A pipeline step calling another method
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public class LambdaProcessingStep<TIn, TResult> : ProcessingStep<TIn, TResult> {
        private readonly Func<TIn, Task<TResult>> _callback;

        /// <summary>
        /// Creates a new instance of <see cref="LambdaProcessingStep{TIn,TResult}"/>
        /// </summary>
        /// <param name="callback"></param>
        public LambdaProcessingStep(Func<TIn, Task<TResult>> callback) {
            _callback = callback;
        }

        /// <inheritdoc />
        public override Task<TResult> InvokeAsync(TIn value) {
            return _callback.Invoke(value);
        }

        /// <summary>
        /// Converts a method to a <see cref="LambdaProcessingStep{TIn,TResult}"/>
        /// </summary>
        /// <param name="next"></param>
        public static implicit operator LambdaProcessingStep<TIn, TResult>(Func<TIn, Task<TResult>> next) {
            return new LambdaProcessingStep<TIn, TResult>(next);
        }
    }
}
