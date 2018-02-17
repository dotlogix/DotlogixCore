// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NullableHelper.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Diagnostics;
#endregion

namespace DotLogix.Core.Extensions {
    /// <summary>
    ///     Helper actions to unify usage of nullable structures and classes.
    /// </summary>
    public static class NullableHelper {
        private static readonly Type NullableType = typeof(Nullable<>).GetGenericTypeDefinition();

        /// <summary>
        ///     Verifies nullable for null value for both reference and value types
        /// </summary>
        /// <returns> True if value is not a null </returns>
        public static bool IsNotNull(this object nullable) {
            return nullable != null /*&& !nullable.Equals(null)*/;
        }

        /// <summary>
        ///     Verifies nullable value for null for both reference and value types
        /// </summary>
        /// <returns> True if value is null </returns>
        public static bool IsNull(this object nullable) {
            return nullable == null /*|| nullable.Equals(null)*/;
        }

        /// <summary>
        ///     Casts nullable value to value and back. Both in and out types should be the same or one of them should be Nullable
        ///     wrapper of the other one. If output type is nullable, input value can be null.
        /// </summary>
        /// <exception cref="NullReferenceException">
        ///     Throws NullReferenceException if converting from null value to non-nullable.
        ///     Verify with IsNull() before
        /// </exception>
        /// <exception cref="InvalidCastException">
        ///     Throws InvalidCastException if using different types instead of nullable and
        ///     non-nullable of the same type
        /// </exception>
        /// <returns> converted value </returns>
        public static T Cast<T>(object value) {
            CanCast(typeof(T), value);
            return (T)value;
        }

        /// <summary>
        ///     Verifies compatibility between specified types when using this helper
        /// </summary>
        /// <remarks>
        ///     the types are compatible if: one type is value type and another one is Nullable wrapper of the first one or both
        ///     types are the same (for reference types)
        /// </remarks>
        /// <returns> </returns>
        public static bool AreTypesCompatible(Type type1, Type type2) {
            if(type1 == type2)
                return true;
            if(type1.IsGenericType && (type1.GetGenericTypeDefinition() == NullableType))
                type1 = Nullable.GetUnderlyingType(type1);
            if(type2.IsGenericType && (type2.GetGenericTypeDefinition() == NullableType))
                type2 = Nullable.GetUnderlyingType(type2);
            return type1 == type2;
        }

        /// <summary>
        ///     Verifies compatibility between specified types when using this helper
        /// </summary>
        /// <remarks>
        ///     the types are compatible if: one type is value type and another one is Nullable wrapper of the first one or both
        ///     types are the same (for reference types)
        /// </remarks>
        /// <returns> </returns>
        public static bool AreTypesCompatible<T1, T2>() {
            return AreTypesCompatible(typeof(T1), typeof(T2));
        }

        /// <summary>
        ///     Verifies is type nullable.
        /// </summary>
        /// <remarks>
        ///     the type is nullable if it is reference type or value type wrapped with Nullable
        /// </remarks>
        /// <returns> </returns>
        public static bool IsNullable(this Type type) {
            return !type.IsValueType || (type.IsGenericType && (type.GetGenericTypeDefinition() == NullableType));
        }

        /// <summary>
        ///     DEBUG only version of AreTypesCompatible
        /// </summary>
        /// <exception cref="InvalidCastException">throws if types are incompatible</exception>
        [Conditional("DEBUG")]
        public static void ValidateTypesCompatibility(Type type1, Type type2) {
            if(!AreTypesCompatible(type1, type2))
                throw new InvalidCastException();
        }

        /// <summary>
        ///     DEBUG only version of AreTypesCompatible
        /// </summary>
        /// <exception cref="InvalidCastException">throws if types are incompatible</exception>
        [Conditional("DEBUG")]
        public static void ValidateTypesCompatibility<T1, T2>() {
            if(!AreTypesCompatible<T1, T2>()) {
                throw new InvalidCastException("Types are incompatible for usage with NullableHelper. [ " +
                                               typeof(T1).ReadableName() + " vs " + typeof(T2).ReadableName() + "]");
            }
        }

        /// <summary>
        ///     DEBUG only version of IsNullable
        /// </summary>
        /// <exception cref="InvalidCastException">throws if type is not nullable</exception>
        [Conditional("DEBUG")]
        public static void ValidateNullable<T>() {
            if(!IsNullable(typeof(T)))
                throw new InvalidCastException("Nullable type required. [" + typeof(T).ReadableName() + "]");
        }

        [Conditional("DEBUG")]
        private static void CanCast(Type type, object value) {
            if(value == null) {
                if(IsNullable(type))
                    return;
                throw new InvalidCastException("Attempt to assign null to non-nullable type [" + type.ReadableName() +
                                               "]. Use IsNull() to check value.");
            }
            ValidateTypesCompatibility(type, value.GetType());
        }
    }
}
