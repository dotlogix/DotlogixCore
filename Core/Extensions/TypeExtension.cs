﻿// ==================================================
// Copyright 2018(C) , DotLogix
// File:  TypeExtension.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Extensions {
    public static class TypeExtension {
        public static bool IsNullableType(this Type type) {
            return type.IsGenericType && !type.IsGenericTypeDefinition &&
                   (type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        public static string GetFriendlyName(this Type type) {
            var friendlyName = type.Name;
            if(type.IsGenericType == false)
                return friendlyName;

            var iBacktick = friendlyName.IndexOf('`');
            if(iBacktick <= 0)
                return friendlyName;
            friendlyName = friendlyName.Remove(iBacktick);
            var generic = type.GetGenericArguments().Length;
            friendlyName += $"<{new string(',', generic - 1)}>";
            return friendlyName;
        }

        public static string GetFriendlyGenericName(this Type type) {
            var friendlyName = type.Name;
            if(type.IsGenericType == false)
                return friendlyName;

            var iBacktick = friendlyName.IndexOf('`');
            if(iBacktick <= 0)
                return friendlyName;

            friendlyName = friendlyName.Remove(iBacktick);
            friendlyName += "<";
            var typeParameters = type.GetGenericArguments().Select(GetFriendlyName);
            friendlyName += string.Join(", ", typeParameters);
            friendlyName += ">";
            return friendlyName;
        }

        public static bool IsAssignableTo(this Type sourceType, Type targetType) {
            return targetType.IsAssignableFrom(sourceType);
        }

        public static bool IsAssignableTo<TTarget>(this Type sourceType) {
            return sourceType.IsAssignableTo(typeof(TTarget));
        }

        public static bool IsEnumerable(this Type sourceType) {
            return IsEnumerable(sourceType, out _);
        }

        public static bool IsEnumerable(this Type sourceType, out Type elementType) {
            if(sourceType.IsArray) {
                elementType = sourceType.GetElementType();
                return true;
            }

            if(sourceType.IsAssignableToOpenGeneric(typeof(IEnumerable<>), out var genericTypeArguments)) {
                elementType = genericTypeArguments[0];
                return true;
            }

            if(sourceType.IsAssignableTo(typeof(IEnumerable))) {
                elementType = typeof(object);
                return true;
            }

            elementType = null;
            return false;
        }

        public static bool IsAssignableToOpenGeneric(this Type sourcetype, Type targetType) {
            if((sourcetype.IsConstructedGenericType == false) || (targetType.IsGenericTypeDefinition == false))
                return false;

            var genericTypeDefinition = sourcetype.GetGenericTypeDefinition();
            if(genericTypeDefinition == targetType)
                return true;

            var targetCandidates = targetType.IsInterface
                                       ? GetInterfacesAssignableTo(sourcetype, true, true)
                                       : GetBaseTypes(sourcetype, true, true);
            return targetCandidates.Any(candidate => candidate == targetType);
        }

        public static bool IsAssignableToOpenGeneric(this Type sourcetype, Type targetType,
                                                     out Type[] genericTypeArguments) {
            genericTypeArguments = null;
            if((sourcetype.IsConstructedGenericType == false) || (targetType.IsGenericTypeDefinition == false))
                return false;


            var genericTypeDefinition = sourcetype.GetGenericTypeDefinition();
            if(genericTypeDefinition == targetType) {
                genericTypeArguments = sourcetype.GetGenericArguments();
                return true;
            }

            var targetCandidates = targetType.IsInterface
                                       ? GetInterfacesAssignableTo(sourcetype, false, true)
                                       : GetBaseTypes(sourcetype, false, true);
            var genericTargetType =
            targetCandidates.FirstOrDefault(candidate => candidate.GetGenericTypeDefinition() == targetType);
            if(genericTargetType == null)
                return false;

            genericTypeArguments = genericTargetType.GetGenericArguments();
            return true;
        }

        public static IEnumerable<Type> GetBaseTypes(this Type type, bool autoOpenGenericType = false,
                                                     bool genericOnly = false) {
            type = type.BaseType;
            while(type != null) {
                if(autoOpenGenericType && type.IsGenericType)
                    yield return type.GetGenericTypeDefinition();
                else if(genericOnly == false)
                    yield return type;
                type = type.BaseType;
            }
        }

        public static IEnumerable<Type> GetTypesAssignableTo(this Type type, bool autoOpenGenericType = false,
                                                             bool genericOnly = false) {
            var baseInterfaces = GetInterfacesAssignableTo(type, autoOpenGenericType, genericOnly);
            var baseTypes = GetBaseTypes(type, autoOpenGenericType, genericOnly);
            return baseTypes.Concat(baseInterfaces);
        }

        public static IEnumerable<Type> GetInterfacesAssignableTo(this Type type, bool autoOpenGenericType = false,
                                                                  bool genericOnly = false) {
            IEnumerable<Type> interfaces = type.GetInterfaces();
            if(genericOnly) {
                interfaces = interfaces.Where(i => i.IsGenericType);
                if(autoOpenGenericType)
                    interfaces = interfaces.Select(i => i.GetGenericTypeDefinition());
            } else if(autoOpenGenericType)
                interfaces = interfaces.Select(OpenIfGenericType);

            return interfaces;
        }

        public static Type OpenIfGenericType(Type type) {
            return type.IsGenericType ? type.GetGenericTypeDefinition() : type;
        }

        public static DataType ToDataType(this Type type) {
            return DataTypeConverter.Instance.GetDataType(type);
        }

        public static object GetDefaultValue(this Type type) {
            return type.IsValueType ? type.CreateDefaultCtor()?.Invoke() : null;
        }

        public static object Instantiate(this Type type) {
            return type.CreateDefaultCtor()?.Invoke();
        }
    }
}
