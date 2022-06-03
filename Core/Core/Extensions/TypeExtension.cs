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
using System.Collections.Concurrent;
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
        private static readonly ConcurrentDictionary<Type, object> DefaultValues = new();
        private static readonly Dictionary<Type, string> PrimitiveTypeNames = new() {
            {typeof(byte), "byte"},
            {typeof(sbyte), "sbyte"},
            {typeof(short), "short"},
            {typeof(ushort), "ushort"},
            {typeof(int), "int"},
            {typeof(uint), "uint"},
            {typeof(long), "long"},
            {typeof(ulong), "ulong"},
            {typeof(float), "float"},
            {typeof(double), "double"},
            {typeof(decimal), "decimal"},
            {typeof(bool), "bool"},
            {typeof(char), "char"},
            
            {typeof(byte?), "byte?"},
            {typeof(sbyte?), "sbyte?"},
            {typeof(short?), "short?"},
            {typeof(ushort?), "ushort?"},
            {typeof(int?), "int?"},
            {typeof(uint?), "uint?"},
            {typeof(long?), "long?"},
            {typeof(ulong?), "ulong?"},
            {typeof(float?), "float?"},
            {typeof(double?), "double?"},
            {typeof(decimal?), "decimal?"},
            {typeof(bool?), "bool?"},
            {typeof(char?), "char?"},
        
            {typeof(object), "object"},
            {typeof(string), "string"},
            {typeof(void), "void"}
        };

        /// <summary>
        ///     Gets the properties of a type ordered by the inheritance level (deepest first)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertiesByInheritance(this Type type) {
            var declaredOnly = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            foreach(var property in type.GetProperties(declaredOnly)) {
                yield return property;
            }

            foreach(var baseType in type.GetBaseTypes()) {
                foreach (var property in baseType.GetProperties(declaredOnly)) {
                    yield return property;
                }
            }
        }

        /// <summary>
        ///     Gets all base types of a specified type
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="asTypeDefinition">get the open generic type definition of the basetypes</param>
        /// <param name="genericOnly">Only return generic types</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetBaseTypes(this Type type, bool asTypeDefinition = false, bool genericOnly = false) {
            type = type.BaseType;
            while(type != null) {
                if(asTypeDefinition && type.IsGenericType)
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
        /// <param name="asTypeDefinition">get the open generic type definition of the basetypes</param>
        /// <param name="genericOnly">Only return generic types</param>
        public static IEnumerable<Type> GetTypesAssignableTo(this Type type, bool asTypeDefinition = false, bool genericOnly = false) {
            var baseInterfaces = GetInterfacesAssignableTo(type, asTypeDefinition, genericOnly);
            var baseTypes = GetBaseTypes(type, asTypeDefinition, genericOnly);
            return baseTypes.Concat(baseInterfaces);
        }

        /// <summary>
        ///     Gets all interfaces a specified type is assignable to
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="asTypeDefinition">If the type is generic return the generic type definition</param>
        /// <param name="genericOnly">Only return generic types</param>
        public static IEnumerable<Type> GetInterfacesAssignableTo(this Type type, bool asTypeDefinition = false, bool genericOnly = false) {
            var interfaces = type.GetInterfaces().AsEnumerable();
            if(genericOnly)
                interfaces = interfaces.Where(i => i.IsGenericType);

            if(asTypeDefinition)
                interfaces = interfaces.Select(TryGetGenericTypeDefinition);

            return interfaces;
        }

        /// <summary>
        ///     Get the generic type definition of a type if it is generic
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns></returns>
        public static Type TryGetGenericTypeDefinition(this Type type) {
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
            return type.IsClass == false
                       ? DefaultValues.GetOrAdd(type, Instantiate)
                       : null;
        }

        #region TypeCheck
        /// <summary>
        ///     Determines if a type is a nullable type
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns></returns>
        public static bool IsNullable(this Type type) {
            return type.IsGenericType && !type.IsGenericTypeDefinition && (type.GetGenericTypeDefinition() == typeof(Nullable<>));
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

            if(sourceType.IsAssignableToGeneric(typeof(IEnumerable<>), out var genericTypeArguments)) {
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
            if(type == null) {
                return null;
            }

            if(PrimitiveTypeNames.TryGetValue(type, out var typeName)) {
                return typeName;
            }
            
            typeName = type.Name;
            if(type.IsGenericType == false) {
                return typeName;
            }

            if(type.IsAssignableToGeneric(typeof(Nullable<>), out var genericArguments)) {
                return GetFriendlyName(genericArguments[0]) + "?";
            }

            genericArguments = type.GetGenericArguments();
            var index = typeName.IndexOf('`');
            var sb = new StringBuilder(typeName, 0, index, index + genericArguments.Length + 1);
            sb.Append('<');
            sb.Append(',', genericArguments.Length - 1);
            sb.Append('>');
            return sb.ToString();
        }

        /// <summary>
        ///     Gets a more readable name for generic types (including the the generic argument names) or the type name if the type
        ///     is not generic.
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns></returns>
        public static string GetFriendlyGenericName(this Type type) {
            return GetFriendlyGenericName(type, false, "<", ">");
        }

        /// <summary>
        ///     Gets a more readable name for generic types (including the the generic argument names) or the type name if the type
        ///     is not generic.
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="includeNamespace"></param>
        /// <param name="genericEnclosureStart"></param>
        /// <param name="genericEnclosureEnd"></param>
        /// <returns></returns>
        public static string GetFriendlyGenericName(this Type type, bool includeNamespace, string genericEnclosureStart, string genericEnclosureEnd) {
            if(type == null) {
                return null;
            }

            if(PrimitiveTypeNames.TryGetValue(type, out var typeName)) {
                return includeNamespace ? string.Concat(type.Namespace, ".", typeName) : typeName;
            }
            
            if(type.IsGenericParameter) {
                return type.Name;
            }
            
            typeName = includeNamespace ? string.Concat(type.Namespace, ".", type.Name) : type.Name;
            if(type.IsGenericType == false) {
                return typeName;
            }

            if(type.IsAssignableToGeneric(typeof(Nullable<>), out var genericArguments)) {
                return GetFriendlyGenericName(genericArguments[0]) + "?";
            }

            genericArguments = type.GetGenericArguments();
            var count = typeName.IndexOf('`');
            var sb = new StringBuilder(typeName, 0, count, typeName.Length);
            sb.Append(genericEnclosureStart);
            var typeParameters = genericArguments.Select(t => GetFriendlyGenericName(t, includeNamespace, genericEnclosureStart, genericEnclosureEnd));
            sb.AppendJoin(", ", typeParameters);
            sb.Append(genericEnclosureEnd);
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
        public static bool IsAssignableToGeneric(this Type sourceType, Type targetType) {
            if(targetType.IsGenericTypeDefinition == false)
                return false;

            if(sourceType.IsConstructedGenericType) {
                var genericTypeDefinition = sourceType.GetGenericTypeDefinition();
                if(genericTypeDefinition == targetType)
                    return true;
            }
            
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
        public static bool IsAssignableToGeneric(this Type sourceType, Type targetType, out Type[] genericTypeArguments) {
            genericTypeArguments = null;
            if(targetType.IsGenericTypeDefinition == false)
                return false;


            if(sourceType.IsConstructedGenericType) {
                var genericTypeDefinition = sourceType.GetGenericTypeDefinition();
                if(genericTypeDefinition == targetType) {
                    genericTypeArguments = sourceType.GetGenericArguments();
                    return true;
                }
            }

            var targetCandidates = targetType.IsInterface
                                       ? GetInterfacesAssignableTo(sourceType, genericOnly: true)
                                       : GetBaseTypes(sourceType, genericOnly: true);
            var genericTargetType = targetCandidates.FirstOrDefault(candidate => candidate.GetGenericTypeDefinition() == targetType);
            if(genericTargetType == null)
                return false;

            genericTypeArguments = genericTargetType.GetGenericArguments();
            return true;
        }
        #endregion

        #region MyRegion
        /// <inheritdoc cref="Type.GetMethod(string, Type[])"/>
        public static MethodInfo GetMethod(this Type sourceType, string methodName, params Type[] typeDefinitions) {
            return sourceType.GetMethod(methodName, typeDefinitions);
        }
        
        /// <inheritdoc cref="Type.GetMethod(string, Type[])"/>
        public static MethodInfo GetGenericMethod(this Type sourceType, string methodName, params Type[] typeDefinitions) {
            foreach(var candidate in sourceType.GetRuntimeMethods().Where(m => m.IsGenericMethod && m.Name == methodName)) {
                var parameters = candidate.GetParameters();
                if(parameters.Length != typeDefinitions.Length)
                    continue;

                var valid = true;
                for(var i = 0; i < parameters.Length && valid; i++) {
                    var expectedType = typeDefinitions[i];
                    var parameterType = parameters[i].ParameterType;

                    valid = expectedType.IsGenericTypeDefinition
                                ? parameterType.IsAssignableToGeneric(expectedType)
                                : parameterType.IsAssignableTo(expectedType);
                }

                if(valid) {
                    return candidate;
                }
            }

            return null;
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
