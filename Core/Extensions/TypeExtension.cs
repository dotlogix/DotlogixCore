// ==================================================
// Copyright 2018(C) , DotLogix
// File:  TypeExtension.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Extensions {
    /// <summary>
    /// A static class providing extension methods for <see cref="Type"/>
    /// </summary>
    public static class TypeExtension {
        /// <summary>
        ///     Gets the properties of a type ordered by the inheritance level (deepest first)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertiesByInheritance(this Type type) {
            var types = type.GetBaseTypes().ToList();
            types.Reverse();
            types.Add(type);
            return types.SelectMany(t => t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)).ToArray();
        }

        /// <summary>
        ///     Gets all base types of a specified type
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="autoOpenGenericType">get the open generic type definition of the basetypes</param>
        /// <param name="genericOnly">Only return generic types</param>
        /// <returns></returns>
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

        /// <summary>
        ///     Gets all types a specified type is assignable to
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="autoOpenGenericType">get the open generic type definition of the basetypes</param>
        /// <param name="genericOnly">Only return generic types</param>
        public static IEnumerable<Type> GetTypesAssignableTo(this Type type, bool autoOpenGenericType = false,
                                                             bool genericOnly = false) {
            var baseInterfaces = GetInterfacesAssignableTo(type, autoOpenGenericType, genericOnly);
            var baseTypes = GetBaseTypes(type, autoOpenGenericType, genericOnly);
            return baseTypes.Concat(baseInterfaces);
        }

        /// <summary>
        ///     Gets all interfaces a specified type is assignable to
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="autoOpenGenericType">get the open generic type definition of the basetypes</param>
        /// <param name="genericOnly">Only return generic types</param>
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

        /// <summary>
        ///     Get the generic type definition of a type if it is generic
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns></returns>
        public static Type OpenIfGenericType(this Type type) {
            return type.IsGenericType ? type.GetGenericTypeDefinition() : type;
        }

        /// <summary>
        ///     Returns the data type of a type for more advanced informations about a type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DataType ToDataType(this Type type) {
            return DataTypeConverter.Instance.GetDataType(type);
        }

        /// <summary>
        ///     Returns the default value of a type
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns></returns>
        public static object GetDefaultValue(this Type type) {
            return type.IsValueType ? type.CreateDefaultCtor()?.Invoke() : null;
        }

        #region TypeCheck
        /// <summary>
        ///     Determines if a type is a nullable type
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns></returns>
        public static bool IsNullableType(this Type type) {
            return type.IsGenericType && !type.IsGenericTypeDefinition &&
                   (type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        ///     Determines if a type is an enumerable type, also returns the element type if possible
        /// </summary>
        /// <param name="sourceType">The type to check</param>
        /// <param name="elementType">The element type of the enumerable</param>
        /// <returns></returns>
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

        /// <summary>
        ///     Determines if a type is an enumerable type
        /// </summary>
        /// <param name="sourceType">The type to check</param>
        /// <returns></returns>
        public static bool IsEnumerable(this Type sourceType) {
            return IsEnumerable(sourceType, out _);
        }
        #endregion

        #region GetFriendlyName
        /// <summary>
        ///     Gets a more readable name for generic types or the type name if the type is not generic
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns></returns>
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

        /// <summary>
        ///     Gets a more readable name for generic types (including the the generic argument names) or the type name if the type
        ///     is not generic.
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns></returns>
        public static string GetFriendlyGenericName(this Type type) {
            var friendlyName = type.Name;
            if(type.IsGenericType == false)
                return friendlyName;

            var genericArguments = type.GetGenericArguments();
            var sb = new StringBuilder(friendlyName, 0, friendlyName.Length - genericArguments.Length, friendlyName.Length);
            sb.Append('<');
            var typeParameters = type.GetGenericArguments().Select(GetFriendlyName);
            sb.Append(string.Join(", ", typeParameters));
            sb.Append('>');
            return sb.ToString();
        }
        #endregion

        #region IsAssignableTo
        /// <summary>
        ///     Determines if a type is assignable to another type
        /// </summary>
        /// <param name="sourceType">The type to check</param>
        /// <param name="targetType">The target type</param>
        /// <returns></returns>
        public static bool IsAssignableTo(this Type sourceType, Type targetType) {
            return targetType.IsAssignableFrom(sourceType);
        }

        /// <summary>
        ///     Determines if a type is assignable to another type
        /// </summary>
        /// <param name="sourceType">The type to check</param>
        /// <typeparam name="TTarget">The target type</typeparam>
        /// <returns></returns>
        public static bool IsAssignableTo<TTarget>(this Type sourceType) {
            return sourceType.IsAssignableTo(typeof(TTarget));
        }

        /// <summary>
        ///     Determines if a type is assignable to an open generic type
        /// </summary>
        /// <param name="sourceType">The type to check</param>
        /// <param name="targetType">The open generic type</param>
        /// <returns></returns>
        public static bool IsAssignableToOpenGeneric(this Type sourceType, Type targetType) {
            if((sourceType.IsConstructedGenericType == false) || (targetType.IsGenericTypeDefinition == false))
                return false;

            var genericTypeDefinition = sourceType.GetGenericTypeDefinition();
            if(genericTypeDefinition == targetType)
                return true;

            var targetCandidates = targetType.IsInterface
                                       ? GetInterfacesAssignableTo(sourceType, true, true)
                                       : GetBaseTypes(sourceType, true, true);
            return targetCandidates.Any(candidate => candidate == targetType);
        }

        /// <summary>
        ///     Determines if a type is assignable to an open generic type, also returns the generic arguments of the constructed
        ///     generic type
        /// </summary>
        /// <param name="sourceType">The type to check</param>
        /// <param name="targetType">The open generic type</param>
        /// <param name="genericTypeArguments">The generic arguments of the constructed generic type</param>
        /// <returns></returns>
        public static bool IsAssignableToOpenGeneric(this Type sourceType, Type targetType,
                                                     out Type[] genericTypeArguments) {
            genericTypeArguments = null;
            if((sourceType.IsConstructedGenericType == false) || (targetType.IsGenericTypeDefinition == false))
                return false;


            var genericTypeDefinition = sourceType.GetGenericTypeDefinition();
            if(genericTypeDefinition == targetType) {
                genericTypeArguments = sourceType.GetGenericArguments();
                return true;
            }

            var targetCandidates = targetType.IsInterface
                                       ? GetInterfacesAssignableTo(sourceType, false, true)
                                       : GetBaseTypes(sourceType, false, true);
            var genericTargetType =
            targetCandidates.FirstOrDefault(candidate => candidate.GetGenericTypeDefinition() == targetType);
            if(genericTargetType == null)
                return false;

            genericTypeArguments = genericTargetType.GetGenericArguments();
            return true;
        }
        #endregion

        #region Instantiate
        /// <summary>
        ///     Creates a new instance of this type using the default constructor
        /// </summary>
        /// <param name="type">The type to instantiate</param>
        /// <returns></returns>
        public static object Instantiate(this Type type) {
            return type.CreateDefaultCtor()?.Invoke();
        }

        /// <summary>
        ///     Creates a new instance of this type using the constructor with matching parameters
        /// </summary>
        /// <param name="type">The type to instantiate</param>
        /// <param name="parameters">The parameters used for invoking the constructor</param>
        /// <returns></returns>
        public static object Instantiate(this Type type, params object[] parameters) {
            var typeArray = ToTypeArray(parameters);
            return type.CreateDynamicCtor(typeArray)?.Invoke(parameters);
        }

        /// <summary>
        ///     Creates a new instance of a generic type using the provided generic arguments using the default constructor
        /// </summary>
        /// <param name="genericType">The type to instantiate</param>
        /// <param name="genericArguments">The generic arguments to build the type</param>
        /// <returns></returns>
        public static object Instantiate(this Type genericType, params Type[] genericArguments) {
            if(genericType.IsGenericTypeDefinition == false)
                throw new ArgumentException("Type has to be an open generic", nameof(genericType));
            return genericType.MakeGenericType(genericArguments).Instantiate();
        }

        /// <summary>
        ///     Creates a new instance of a generic type using the provided generic arguments using the constructor with matching
        ///     parameters
        /// </summary>
        /// <param name="genericType">The type to instantiate</param>
        /// <param name="genericArguments">The generic arguments to build the type</param>
        /// <param name="parameters">The parameters used to invoke the constructor</param>
        public static object Instantiate(this Type genericType, Type[] genericArguments, params object[] parameters) {
            if(genericType.IsGenericTypeDefinition == false)
                throw new ArgumentException("Type has to be an open generic", nameof(genericType));
            return genericType.MakeGenericType(genericArguments).Instantiate(parameters);
        }

        /// <summary>
        ///     Creates a new instance of this type using the default constructor
        /// </summary>
        /// <param name="type">The type to instantiate</param>
        /// <returns></returns>
        public static TInstance Instantiate<TInstance>(this Type type) {
            return type.Instantiate() is TInstance instance ? instance : default;
        }

        /// <summary>
        ///     Creates a new instance of this type using the constructor with matching parameters
        /// </summary>
        /// <param name="type">The type to instantiate</param>
        /// <param name="parameters">The parameters used for invoking the constructor</param>
        /// <returns></returns>
        public static TInstance Instantiate<TInstance>(this Type type, params object[] parameters) {
            return type.Instantiate(parameters) is TInstance instance ? instance : default;
        }

        /// <summary>
        ///     Creates a new instance of a generic type using the provided generic arguments using the default constructor
        /// </summary>
        /// <param name="genericType">The type to instantiate</param>
        /// <param name="genericArguments">The generic arguments to build the type</param>
        /// <returns></returns>
        public static TInstance Instantiate<TInstance>(this Type genericType, params Type[] genericArguments) {
            return genericType.Instantiate(genericArguments) is TInstance instance ? instance : default;
        }

        /// <summary>
        ///     Creates a new instance of a generic type using the provided generic arguments using the constructor with matching
        ///     parameters
        /// </summary>
        /// <param name="genericType">The type to instantiate</param>
        /// <param name="genericArguments">The generic arguments to build the type</param>
        /// <param name="parameters">The parameters used to invoke the constructor</param>
        public static TInstance Instantiate<TInstance>(this Type genericType, Type[] genericArguments, params object[] parameters) {
            return genericType.Instantiate(genericArguments, parameters) is TInstance instance ? instance : default;
        }
        #endregion

        #region ToTypeArray
        /// <summary>
        ///     Generates an array of the types of a list of objects
        /// </summary>
        /// <param name="objects">The objects</param>
        /// <returns></returns>
        public static Type[] ToTypeArray(this IReadOnlyList<object> objects) {
            var typeArray = new Type[objects.Count];
            for(var i = 0; i < objects.Count; i++)
                typeArray[i] = objects[i].GetType();
            return typeArray;
        }

        /// <summary>
        ///     Generates an array of the types of a list of objects
        /// </summary>
        /// <param name="objects">The objects</param>
        /// <returns></returns>
        public static Type[] ToTypeArray(this IEnumerable<object> objects) {
            return objects.Select(o => o.GetType()).ToArray();
        }
        #endregion
    }
}
