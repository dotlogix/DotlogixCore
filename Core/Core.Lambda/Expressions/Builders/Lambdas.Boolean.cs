// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  Lambdas.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 09.06.2021 00:02
// LastEdited:  26.09.2021 22:15
// ==================================================

using System.Linq.Expressions;

namespace DotLogix.Core.Expressions {
    public static partial class Lambdas {
        #region Boolean
        public static Lambda<bool> IsFalse(this Lambda<bool> instance) {
            return Expression.IsFalse(instance.Body);
        }

        public static Lambda<bool> IsTrue(this Lambda<bool> instance) {
            return Expression.IsTrue(instance.Body);
        }

        public static Lambda<bool> AndAlso(this Lambda<bool> instance, Lambda<bool> other) {
            return Expression.AndAlso(instance.Body, other.Body);
        }

        public static Lambda<bool> OrElse(this Lambda<bool> instance, Lambda<bool> other) {
            return Expression.OrElse(instance.Body, other.Body);
        }
        #endregion
    }
}
