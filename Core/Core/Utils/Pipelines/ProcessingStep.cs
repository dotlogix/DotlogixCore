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

namespace DotLogix.Core.Utils.Pipelines; 

/// <summary>
/// A base class for pipeline processing steps
/// </summary>
/// <typeparam name="TIn"></typeparam>
/// <typeparam name="TResult"></typeparam>
public abstract class ProcessingStep<TIn, TResult> {
    /// <summary>
    /// Invoke the step action
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public abstract Task<TResult> InvokeAsync(TIn value);
}

/// <summary>
/// A base class for pipeline processing steps
/// </summary>
/// <typeparam name="TIn">The step input</typeparam>
/// <typeparam name="TResult">The pipeline result</typeparam>
/// <typeparam name="TNext">The input value of the next step</typeparam>
public abstract class ProcessingStep<TIn, TNext, TResult> : ProcessingStep<TIn, TResult> {
    private ProcessingStep<TNext, TResult> _nextStep;

    /// <summary>
    /// Calls the next pipeline step if existing
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">If no following step is available it is required to return a value</exception>
    protected virtual Task<TResult> NextAsync(TNext value) {
        if(_nextStep == null)
            throw new InvalidOperationException("This step must return a value or define a next step");
        return _nextStep.InvokeAsync(value);
    }

    /// <summary>
    /// End a multi operational pipeline step
    /// </summary>
    /// <param name="next"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public virtual ProcessingStep<TIn, TResult> EndWith(ProcessingStep<TNext, TResult> next) {
        _nextStep = next;
        return this;
    }

    /// <summary>
    /// Add a pipeline step after the current
    /// </summary>
    /// <param name="next"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public virtual ProcessingStep<TIn, T, TResult> ContinueWith<T>(ProcessingStep<TNext, T, TResult> next) {
        _nextStep = next;
        return new WrappedProcessingStep<TIn, TNext, T, TResult>(this, next);
    }

    /// <summary>
    /// End a multi operational pipeline step
    /// </summary>
    /// <param name="next"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public ProcessingStep<TIn, TResult> EndWith(Func<TNext, Task<TResult>> next) {
        var nextStep = new LambdaProcessingStep<TNext, TResult>(next);
        return EndWith(nextStep);
    }
    /// <summary>
    /// Add a pipeline step after the current
    /// </summary>
    /// <param name="next"></param>
    /// <returns></returns>
    public ProcessingStep<TIn, TNext, TResult> ContinueWith(Func<TNext, Func<TNext, Task<TResult>>, Task<TResult>> next) {
        return ContinueWith<TNext>(next);
    }
    /// <summary>
    /// Add a pipeline step after the current
    /// </summary>
    /// <param name="next"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public ProcessingStep<TIn, T, TResult> ContinueWith<T>(Func<TNext, Func<T, Task<TResult>>, Task<TResult>> next) {
        var nextStep = new LambdaProcessingStep<TNext, T, TResult>(next);
        return ContinueWith(nextStep);
    }
}