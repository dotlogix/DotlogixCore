﻿// ==================================================
// Copyright 2016(C) , DotLogix
// File:  FluentDynamics.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
using System.Reflection;
using DotLogix.Core.Reflection.Delegates;
using DotLogix.Core.Reflection.Fluent;
#endregion

namespace DotLogix.Core.Reflection.Dynamics {
    public static class FluentDynamics {
        #region Type
        public static DynamicType CreateDynamicType(this Type type, MemberTypes includedMemberTypes = MemberTypes.All) {
            return new DynamicType(type, includedMemberTypes);
        }
        #endregion

        #region Invoke
        public static DynamicInvoke CreateDynamicInvoke(this MethodInfo methodInfo) {
            if(methodInfo == null)
                throw new ArgumentNullException(nameof(methodInfo));

            var invokeDelegate = FluentIl.CreateInvoke(methodInfo);
            var access = GetAccessModifiers(methodInfo);
            var visibility = GetVisibilityModifiers(methodInfo);
            return new DynamicInvoke(methodInfo, access, visibility, invokeDelegate);
        }
        #endregion

        #region Constructor
        public static DynamicCtor CreateDynamicCtor(this ConstructorInfo constructorInfo, bool allowNonPublic = true) {
            if(constructorInfo == null)
                throw new ArgumentNullException(nameof(constructorInfo));

            var declaringType = constructorInfo.DeclaringType;
            var ctorDelegate = FluentIl.CreateConstructor(declaringType, constructorInfo, allowNonPublic);
            var access = GetAccessModifiers(constructorInfo);
            var visibility = GetVisibilityModifiers(constructorInfo);
            return new DynamicCtor(declaringType, constructorInfo, access, visibility, ctorDelegate);
        }

        public static DynamicCtor CreateDynamicCtor(this Type declaringType, Type[] parameterTypes,
                                                    bool allowNonPublic = true) {
            if(declaringType == null)
                throw new ArgumentNullException(nameof(declaringType));

            var binderFlags = BindingFlags.Instance | BindingFlags.Public;
            if(allowNonPublic)
                binderFlags |= BindingFlags.NonPublic;

            var constructorInfo = declaringType.GetConstructor(binderFlags, null, parameterTypes, null);
            CtorDelegate ctorDelegate;
            AccessModifiers access;
            VisibilityModifiers visibility;
            if(constructorInfo == null) {
                if((parameterTypes.Length != 0) || (declaringType.IsValueType == false))
                    throw new InvalidOperationException("Constructor is not defined");
                ctorDelegate = FluentIl.CreateConstructor(declaringType, null, allowNonPublic);
                access = GetAccessModifiers(declaringType);
                visibility = GetVisibilityModifiers(declaringType);
            } else {
                ctorDelegate = FluentIl.CreateConstructor(declaringType, constructorInfo, allowNonPublic);
                access = GetAccessModifiers(constructorInfo);
                visibility = GetVisibilityModifiers(constructorInfo);
            }
            return new DynamicCtor(declaringType, null, access, visibility, ctorDelegate);
        }

        public static DynamicCtor CreateDefaultCtor(this Type declaringType, bool allowNonPublic = true) {
            return CreateDynamicCtor(declaringType, Type.EmptyTypes, allowNonPublic);
        }
        #endregion

        #region Getter
        public static DynamicGetter CreateDynamicGetter(this PropertyInfo propertyInfo) {
            if(propertyInfo == null)
                throw new ArgumentNullException(nameof(propertyInfo));

            var getterDelegate = FluentIl.CreateGetter(propertyInfo);
            if(getterDelegate == null)
                return null;
            var getMethod = propertyInfo.GetGetMethod(true);
            var access = GetAccessModifiers(getMethod);
            var visibility = GetVisibilityModifiers(getMethod);
            return new DynamicGetter(access, visibility, getterDelegate);
        }

        public static DynamicGetter CreateDynamicGetter(this FieldInfo fieldInfo) {
            if(fieldInfo == null)
                throw new ArgumentNullException(nameof(fieldInfo));

            var getterDelegate = FluentIl.CreateGetter(fieldInfo);
            if(getterDelegate == null)
                return null;
            var access = GetAccessModifiers(fieldInfo);
            var visibility = GetVisibilityModifiers(fieldInfo);
            return new DynamicGetter(access, visibility, getterDelegate);
        }
        #endregion

        #region Setter
        public static DynamicSetter CreateDynamicSetter(this PropertyInfo propertyInfo) {
            if(propertyInfo == null)
                throw new ArgumentNullException(nameof(propertyInfo));

            var setterDelegate = FluentIl.CreateSetter(propertyInfo);
            if(setterDelegate == null)
                return null;
            var setMethod = propertyInfo.GetSetMethod(true);
            var access = GetAccessModifiers(setMethod);
            var visibility = GetVisibilityModifiers(setMethod);

            return new DynamicSetter(access, visibility, setterDelegate);
        }

        public static DynamicSetter CreateDynamicSetter(this FieldInfo fieldInfo) {
            if(fieldInfo == null)
                throw new ArgumentNullException(nameof(fieldInfo));

            var setterDelegate = FluentIl.CreateSetter(fieldInfo);
            if(setterDelegate == null)
                return null;
            var access = GetAccessModifiers(fieldInfo);
            var visibility = GetVisibilityModifiers(fieldInfo);
            return new DynamicSetter(access, visibility, setterDelegate);
        }
        #endregion

        #region Accessors
        public static DynamicProperty CreateDynamicProperty(this PropertyInfo propertyInfo) {
            if(propertyInfo == null)
                throw new ArgumentNullException(nameof(propertyInfo));
            var getter = CreateDynamicGetter(propertyInfo);
            var setter = CreateDynamicSetter(propertyInfo);
            return new DynamicProperty(propertyInfo, getter, setter);
        }

        public static DynamicField CreateDynamicField(this FieldInfo fieldInfo) {
            if(fieldInfo == null)
                throw new ArgumentNullException(nameof(fieldInfo));
            var getter = CreateDynamicGetter(fieldInfo);
            var setter = CreateDynamicSetter(fieldInfo);
            return new DynamicField(fieldInfo, getter, setter);
        }
        #endregion

        #region Visibility
        public static VisibilityModifiers GetVisibilityModifiers(MethodBase methodBase) {
            var methodAttributes = methodBase.Attributes & MethodAttributes.MemberAccessMask;
            switch(methodAttributes) {
                case MethodAttributes.Private:
                    return VisibilityModifiers.Private;
                case MethodAttributes.Assembly:
                    return VisibilityModifiers.Internal;
                case MethodAttributes.Family:
                    return VisibilityModifiers.Protected;
                case MethodAttributes.FamORAssem:
                    return VisibilityModifiers.ProtectedInternal;
                case MethodAttributes.Public:
                    return VisibilityModifiers.Public;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static VisibilityModifiers GetVisibilityModifiers(Type type) {
            var typeAttributes = type.Attributes & TypeAttributes.VisibilityMask;
            switch(typeAttributes) {
                case TypeAttributes.NotPublic:
                    return VisibilityModifiers.Internal;
                case TypeAttributes.Public:
                    return VisibilityModifiers.Public;
                case TypeAttributes.NestedPublic:
                    return VisibilityModifiers.Public;
                case TypeAttributes.NestedPrivate:
                    return VisibilityModifiers.Private;
                case TypeAttributes.NestedFamily:
                    return VisibilityModifiers.Protected;
                case TypeAttributes.NestedAssembly:
                    return VisibilityModifiers.Internal;
                case TypeAttributes.NestedFamORAssem:
                    return VisibilityModifiers.ProtectedInternal;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static VisibilityModifiers GetVisibilityModifiers(FieldInfo fieldInfo) {
            var fieldAttributes = fieldInfo.Attributes & FieldAttributes.FieldAccessMask;
            switch(fieldAttributes) {
                case FieldAttributes.Private:
                    return VisibilityModifiers.Private;
                case FieldAttributes.Assembly:
                    return VisibilityModifiers.Internal;
                case FieldAttributes.Family:
                    return VisibilityModifiers.Protected;
                case FieldAttributes.FamORAssem:
                    return VisibilityModifiers.ProtectedInternal;
                case FieldAttributes.Public:
                    return VisibilityModifiers.Public;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        #region Access
        public static AccessModifiers GetAccessModifiers(MethodBase methodBase) {
            var methodAttributes = methodBase.Attributes;
            var accessModifiers = AccessModifiers.None;

            if((methodAttributes & MethodAttributes.Static) != 0)
                accessModifiers |= AccessModifiers.Static;

            if((methodAttributes & MethodAttributes.Abstract) != 0)
                accessModifiers |= AccessModifiers.Abstract;

            if((methodAttributes & MethodAttributes.Virtual) != 0)
                accessModifiers |= AccessModifiers.Virtual;

            return accessModifiers;
        }

        public static AccessModifiers GetAccessModifiers(Type type) {
            const TypeAttributes staticAttributes = TypeAttributes.Abstract | TypeAttributes.Sealed;

            var typeAttributes = type.Attributes & TypeAttributes.VisibilityMask;
            var accessModifiers = AccessModifiers.None;

            var abstractOrSealed = typeAttributes & staticAttributes;
            switch(abstractOrSealed) {
                case staticAttributes:
                    accessModifiers |= AccessModifiers.Static;
                    break;
                case TypeAttributes.Abstract:
                    accessModifiers |= AccessModifiers.Abstract;
                    break;
                case TypeAttributes.Sealed:
                    accessModifiers |= AccessModifiers.Sealed;
                    break;
            }

            if(type.IsNested)
                accessModifiers |= AccessModifiers.Nested;

            return accessModifiers;
        }

        public static AccessModifiers GetAccessModifiers(FieldInfo fieldInfo) {
            var fieldAttributes = fieldInfo.Attributes & FieldAttributes.FieldAccessMask;
            var accessModifiers = AccessModifiers.None;

            if((fieldAttributes & FieldAttributes.Static) != 0)
                accessModifiers |= AccessModifiers.Static;

            if((fieldAttributes & FieldAttributes.Literal) != 0)
                accessModifiers |= AccessModifiers.Const;

            return accessModifiers;
        }
        #endregion
    }
}
