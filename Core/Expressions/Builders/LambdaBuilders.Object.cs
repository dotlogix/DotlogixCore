// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  LambdaBuilders.Object.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 07.06.2021 11:49
// LastEdited:  26.09.2021 22:15
// ==================================================

using System;
using System.Linq.Expressions;
using System.Reflection;
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Expressions {
    public static partial class LambdaBuilders {
        public static LambdaBuilder<bool> True => FromValue(true);
        public static LambdaBuilder<bool> False => FromValue(false);
        
        public static LambdaBuilder<T> FromValue<T>(T value) {
            var expression = GetConstantExpression(value, typeof(T));
            return From<T>(expression);
        }
        public static LambdaBuilder FromValue(object value, Type type = null) {
            var expression = GetConstantExpression(value, type);
            return From(expression);
        }

        public static LambdaBuilder From(Expression value) {
            return new LambdaBuilder(value);
        }
        public static LambdaBuilder<T> From<T>(Expression value) {
            if(typeof(T).IsAssignableFrom(value.Type)) {
                return new LambdaBuilder<T>(value);
            }
            throw new ArgumentException(nameof(value), $"Value of type {value.Type} is not assignable to builder type {typeof(T)}");
        }
        
        public static LambdaBuilder<Expression<TDelegate>> Quote<TDelegate>(Expression<TDelegate> value) where TDelegate : Delegate {
            return Expression.Quote(value);
        }
        
        public static LambdaBuilder Parameter(Type type, string name = null) {
            return Expression.Parameter(type, name);
        }
        public static LambdaBuilder<T> Parameter<T>(string name = null) {
            return Expression.Parameter(typeof(T), name);
        }
        
        #region Is
        public static LambdaBuilder IsNull(this LambdaBuilder instance) {
            if(instance.Type.IsClass || instance.Type.IsAssignableToGeneric(typeof(Nullable<>))) {
                return Expression.Equal(instance.Body, Expression.Constant(null, instance.Type));
            }
            return False;
        }
        public static LambdaBuilder<bool> IsFalse(this LambdaBuilder<bool> instance) {
            return Expression.IsFalse(instance.Body);
        }
        public static LambdaBuilder<bool> IsTrue(this LambdaBuilder<bool> instance) {
            return Expression.IsTrue(instance.Body);
        }
        #endregion
        #region Convert
        
        public static LambdaBuilder<T> Coalesce<T>(this LambdaBuilder<T> instance, LambdaBuilder<T> other) {
            return Expression.Coalesce(instance.Body, other.Body);
        }

        public static LambdaBuilder<T> Cast<T>(this LambdaBuilder value) {
            if(value is LambdaBuilder<T> lambdaBuilder) {
                return lambdaBuilder;
            }
            return Cast(value, typeof(T)).Body;
        }
        public static LambdaBuilder Cast(this LambdaBuilder value, Type type) {
            if(value.Type == type) {
                return value;
            }
            return Expression.Convert(value, type);
        }
        
        #endregion
        #region PropertyOrField

        public static LambdaBuilder<TMember> PropertyOrField<TMember>(this LambdaBuilder instance, string name) {
            return Expression.PropertyOrField(instance.Body, name);
        }

        public static LambdaBuilder<TMember> Property<TMember>(this LambdaBuilder instance, string name) {
            return Expression.Property(instance.Body, name);
        }
        public static LambdaBuilder<TMember> Property<TMember>(this LambdaBuilder instance, PropertyInfo propertyInfo) {
            return Expression.Property(instance.Body, propertyInfo);
        }
        
        public static LambdaBuilder<TMember> Field<TMember>(this LambdaBuilder instance, string name) {
            return Expression.Field(instance.Body, name);
        }
        public static LambdaBuilder<TMember> Field<TMember>(this LambdaBuilder instance, FieldInfo fieldInfo) {
            return Expression.Field(instance.Body, fieldInfo);
        }
        #endregion

        private static Expression GetConstantExpression(object value, Type type) {
            type ??= value?.GetType();
            
            if(type == null) {
                throw new ArgumentNullException(nameof(type), "Type is required if value is equal to null");
            }
            
            if(value == null) {
                return Expression.Constant(null, type);
            }

            if(type.IsInstanceOfType(value)) {
                return Expression.Constant(value, type);
            }

            throw new ArgumentException(nameof(value), $"Value of type {value.GetType()} is not assignable to builder type {type}");
        }
    }
}
