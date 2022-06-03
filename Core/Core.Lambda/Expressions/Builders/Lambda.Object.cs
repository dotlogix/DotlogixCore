// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  Lambdas.Object.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 07.06.2021 11:49
// LastEdited:  26.09.2021 22:15
// ==================================================

using System;
using System.Linq.Expressions;
using System.Reflection;
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Expressions {
    public static partial class Lambdas {
        public static Lambda<bool> True => Constant(true);
        public static Lambda<bool> False => Constant(false);
        
        public static (Lambda lambda, ParameterExpression parameter) Parameter(Type type, string name = null) {
            var parameter = Expression.Parameter(type, name);
            return (parameter, parameter);
        }
        public static (Lambda<T> lambda, ParameterExpression parameter) Parameter<T>(string name = null) {
            var parameter = Expression.Parameter(typeof(T), name);
            return (parameter, parameter);
        }

        public static Lambda Constant(object value, Type type = null) {
            return Expression.Constant(value, type ?? value.GetType());
        }
        public static Lambda<T> Constant<T>(T value) {
            return Expression.Constant(value, typeof(T));
        }

        public static Lambda<Expression<TDelegate>> Quote<TDelegate>(Expression<TDelegate> value) where TDelegate : Delegate {
            return Expression.Quote(value);
        }
        
        public static Lambda From(Expression expression) {
            return expression;
        }
        public static Lambda<T> From<T>(Expression expression) {
            return expression;
        }
        
        
        #region Is
        public static Lambda IsNull(this Lambda instance) {
            if(instance.Type.IsClass || instance.Type.IsNullable()) {
                return From<bool>(Expression.Equal(instance.Body, Expression.Constant(null, instance.Type)));
            }
            return False;
        }
        #endregion
        #region Convert
        
        public static Lambda<T> Coalesce<T>(this Lambda<T> instance, Lambda<T> other) {
            return Expression.Coalesce(instance.Body, other.Body);
        }

        public static Lambda<T> Cast<T>(this Lambda value) {
            if(value is Lambda<T> lambda) {
                return lambda;
            }

            return value.Type == typeof(T) ? value.Body : Expression.Convert(value.Body, typeof(T));
        }
        public static Lambda Cast(this Lambda value, Type type) {
            return value.Type == type ? value : Expression.Convert(value.Body, type);
        }
        
        #endregion
        #region PropertyOrField

        public static Lambda<TMember> PropertyOrField<TMember>(this Lambda instance, string name) {
            return Expression.PropertyOrField(instance.Body, name);
        }
        public static Lambda PropertyOrField(this Lambda instance, string name) {
            return Expression.PropertyOrField(instance.Body, name);
        }

        public static Lambda<TMember> Property<TMember>(this Lambda instance, string name) {
            return Expression.Property(instance.Body, name);
        }
        public static Lambda Property(this Lambda instance, string name) {
            return Expression.Property(instance.Body, name);
        }
        
        public static Lambda<TMember> Property<TMember>(this Lambda instance, PropertyInfo propertyInfo) {
            return Expression.Property(instance.Body, propertyInfo);
        }
        public static Lambda Property(this Lambda instance, PropertyInfo propertyInfo) {
            return Expression.Property(instance.Body, propertyInfo);
        }
        
        public static Lambda<TMember> Field<TMember>(this Lambda instance, string name) {
            return Expression.Field(instance.Body, name);
        }
        public static Lambda Field(this Lambda instance, string name) {
            return Expression.Field(instance.Body, name);
        }
        public static Lambda<TMember> Field<TMember>(this Lambda instance, FieldInfo fieldInfo) {
            return Expression.Field(instance.Body, fieldInfo);
        }
        public static Lambda Field(this Lambda instance, FieldInfo fieldInfo) {
            return Expression.Field(instance.Body, fieldInfo);
        }
        #endregion
    }
}
