// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  LambdaBuilder.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 09.06.2021 00:02
// LastEdited:  26.09.2021 22:15
// ==================================================

#region
using System;
using System.Linq.Expressions;
#endregion

namespace DotLogix.Core.Expressions {
    public class LambdaBuilder {
        public Type Type { get; }

        public Expression Body { get; }

        public LambdaBuilder(Expression body, Type type = null) {
            Type = type ?? body.Type;
            Body = Type.IsAssignableFrom(body.Type) ? body : throw new ArgumentException(nameof(body), $"Body of type {body.Type} is not assignable to builder type {type}");;
        }

        public static implicit operator LambdaBuilder(Expression value) => LambdaBuilders.From(value);

        public static implicit operator Expression(LambdaBuilder value) => value.Body;
    }

    public class LambdaBuilder<T> : LambdaBuilder {
        public LambdaBuilder(Expression body) : base(body, typeof(T)) {
        }

        public static implicit operator LambdaBuilder<T>(T value) => LambdaBuilders.FromValue(value);

        public static implicit operator LambdaBuilder<T>(Expression value) => LambdaBuilders.From<T>(value);

        public static implicit operator Expression(LambdaBuilder<T> value) => value.Body;
    }
}
