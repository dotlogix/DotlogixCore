// ==================================================
// Copyright 2018(C) , DotLogix
// File:  PipelineStep.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  19.11.2018
// LastEdited:  20.11.2018
// ==================================================

#region
using System.Threading.Tasks;
#endregion

namespace DotLogix.Core.Patterns {
    public abstract class PipelineStep<TIn, TOut> : IPipelineStep<TIn, TOut> {
        public IPipelineStep<TIn, TOut> NextStep { get; set; }

        public abstract Task<Optional<TOut>> Execute(TIn value);

        protected Task<Optional<TOut>> Next(TIn value) {
            return NextStep != null ? NextStep.Execute(value) : Task.FromResult(Optional<TOut>.Undefined);
        }
    }
}
