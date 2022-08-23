// ==================================================
// Copyright 2018(C) , DotLogix
// File:  FluentIl.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Reflection;
using System.Reflection.Emit;
using DotLogix.Core.Reflection.Delegates;
#endregion

namespace DotLogix.Core.Reflection.Fluent; 

/// <summary>
///     A static class to create delegates
/// </summary>
public static class FluentIl {
    #region Calls
    /// <summary>
    ///     Creates a dynamic method to call a reflected method info with minimum overhead
    /// </summary>
    public static InvokeDelegate CreateInvoke(MethodInfo methodInfo, bool allowNonPublic = true) {
        if(methodInfo == null)
            throw new ArgumentNullException(nameof(methodInfo));

        if(methodInfo.IsGenericMethodDefinition)
            return null;

        if((methodInfo.IsPublic == false) && (allowNonPublic == false))
            return null;

        var dynamicMethod = new DynamicMethod($"Dynamic{methodInfo.Name}", typeof(object),
            new[] {typeof(object), typeof(object[])},
            methodInfo.Module, true);
        var ilGen = dynamicMethod.GetFluentIlGenerator();
        var ps = methodInfo.GetParameters();
        var paramTypes = new Type[ps.Length];
        for(var i = 0; i < paramTypes.Length; i++) {
            if(ps[i].ParameterType.IsByRef)
                paramTypes[i] = ps[i].ParameterType.GetElementType();
            else
                paramTypes[i] = ps[i].ParameterType;
        }

        var locals = new LocalBuilder[paramTypes.Length];

        for(var i = 0; i < paramTypes.Length; i++)
            locals[i] = ilGen.DeclareLocal(paramTypes[i], true);
        for(var i = 0; i < paramTypes.Length; i++) {
            ilGen.
                Ldarg_1().
                Ldc_I4_Auto(i).
                Ldelem_Ref().
                Unbox_AnyOrCast(paramTypes[i]).
                Stloc(locals[i]);
        }

        if(!methodInfo.IsStatic) {
            ilGen.Ldarg_0().
                UnboxOrCast(methodInfo.DeclaringType);
        }

        for(var i = 0; i < paramTypes.Length; i++) {
            if(ps[i].ParameterType.IsByRef)
                ilGen.Ldloca_S(locals[i]);
            else
                ilGen.Ldloc(locals[i]);
        }

        ilGen.CallOrVirt(methodInfo, null);
        if(methodInfo.ReturnType == typeof(void))
            ilGen.Ldnull();
        else
            ilGen.BoxIfNeeded(methodInfo.ReturnType);

        for(var i = 0; i < paramTypes.Length; i++) {
            if(!ps[i].ParameterType.IsByRef)
                continue;
            ilGen.
                Ldarg_1().
                Ldc_I4_Auto(i).
                Ldloc(locals[i]).
                BoxIfNeeded(locals[i].LocalType).
                Stelem_Ref();
        }

        ilGen.Ret();
        return (InvokeDelegate)dynamicMethod.CreateDelegate(typeof(InvokeDelegate));
    }
    #endregion

    #region Constructor
    /// <summary>
    ///     Creates a dynamic method to call a reflected constructor info with minimum overhead
    /// </summary>
    public static CtorDelegate CreateConstructor(Type declaringType, ConstructorInfo constructorInfo,
        bool allowNonPublic = true) {
        if(declaringType == null)
            throw new ArgumentNullException(nameof(declaringType));


        Module module;
        ParameterInfo[] parameters;
        string ctorName;
        if(constructorInfo == null) {
            if(declaringType.IsValueType == false)
                throw new ArgumentNullException(nameof(constructorInfo));
            if((declaringType.IsPublic == false) && (allowNonPublic == false))
                return null;

            module = declaringType.Module;
            parameters = null;
            ctorName = declaringType.Name;
        } else {
            if(constructorInfo.IsGenericMethodDefinition)
                return null;

            if((constructorInfo.IsPublic == false) && (allowNonPublic == false))
                return null;
            module = constructorInfo.Module;
            parameters = constructorInfo.GetParameters();
            ctorName = declaringType.IsArray
                ? $"{declaringType.GetElementType()?.Name}Array{parameters.Length}d"
                : declaringType.Name;
        }

        var dynamicMethod = new DynamicMethod($"DynamicCtor{ctorName}", typeof(object),
            new[] {typeof(object[])},
            module, true);
        var ilGen = dynamicMethod.GetFluentIlGenerator();
        var result = ilGen.DeclareLocal(declaringType);
        if(constructorInfo == null) {
            ilGen.
                Ldloca(result).
                Initobj(declaringType);
        } else {
            var paramTypes = new Type[parameters.Length];
            for(var i = 0; i < paramTypes.Length; i++) {
                if(parameters[i].ParameterType.IsByRef)
                    paramTypes[i] = parameters[i].ParameterType.GetElementType();
                else
                    paramTypes[i] = parameters[i].ParameterType;
            }

            var locals = new LocalBuilder[paramTypes.Length];

            for(var i = 0; i < paramTypes.Length; i++)
                locals[i] = ilGen.DeclareLocal(paramTypes[i], true);
            for(var i = 0; i < paramTypes.Length; i++) {
                ilGen.
                    Ldarg_0().
                    Ldc_I4_Auto(i).
                    Ldelem_Ref().
                    Unbox_AnyOrCast(paramTypes[i]).
                    Stloc(locals[i]);
            }

            for(var i = 0; i < paramTypes.Length; i++) {
                if(parameters[i].ParameterType.IsByRef)
                    ilGen.Ldloca_S(locals[i]);
                else
                    ilGen.Ldloc(locals[i]);
            }

            ilGen.Newobj(constructorInfo)
               .BoxIfNeeded(declaringType)
               .Stloc(result);

            for(var i = 0; i < paramTypes.Length; i++) {
                if(!parameters[i].ParameterType.IsByRef)
                    continue;
                ilGen.
                    Ldarg_0().
                    Ldc_I4_Auto(i).
                    Ldloc(locals[i]).
                    BoxIfNeeded(locals[i].LocalType).
                    Stelem_Ref();
            }
        }

        ilGen.Ldloc(result)
           .Ret();
        return (CtorDelegate)dynamicMethod.CreateDelegate(typeof(CtorDelegate));
    }
    #endregion

    #region Members
    /// <summary>
    ///     Creates a dynamic method to call a getter of a property info with minimum overhead
    /// </summary>
    public static GetterDelegate CreateGetter(PropertyInfo propertyInfo, bool allowNonPublic = true) {
        if(propertyInfo == null)
            throw new ArgumentNullException(nameof(propertyInfo));
        if((propertyInfo.CanRead == false) || propertyInfo.IsIndexerProperty())
            return null;

        var propertyName = propertyInfo.Name;
        var getMethod = propertyInfo.GetGetMethod(allowNonPublic);
        if(getMethod == null)
            return null;
        var declaringType = propertyInfo.DeclaringType;
        var propertyType = propertyInfo.PropertyType;

        var dynamicMethod = new DynamicMethod($"DynamicGet{propertyName}", typeof(object), new[] {typeof(object)},
            getMethod.Module, true);
        var ilGen = dynamicMethod.GetFluentIlGenerator();
        if(getMethod.IsStatic == false) {
            ilGen.
                Ldarg_0().
                UnboxOrCast(declaringType);
        }

        ilGen.
            CallOrVirt(getMethod).
            BoxOrCast(propertyType).
            Ret();
        return (GetterDelegate)dynamicMethod.CreateDelegate(typeof(GetterDelegate));
    }

    /// <summary>
    ///     Creates a dynamic method to call a setter of a property info with minimum overhead
    /// </summary>
    public static SetterDelegate CreateSetter(PropertyInfo propertyInfo, bool allowNonPublic = true) {
        if(propertyInfo == null)
            throw new ArgumentNullException(nameof(propertyInfo));
        if((propertyInfo.CanWrite == false) || propertyInfo.IsIndexerProperty())
            return null;

        var propertyName = propertyInfo.Name;
        var setMethod = propertyInfo.GetSetMethod(allowNonPublic);
        if(setMethod == null)
            return null;
        var declaringType = propertyInfo.DeclaringType;
        var propertyType = propertyInfo.PropertyType;

        var dynamicMethod = new DynamicMethod($"DynamicSet{propertyName}", typeof(void),
            new[] {typeof(object), typeof(object)}, propertyInfo.Module, true);
        var ilGen = dynamicMethod.GetFluentIlGenerator();
        if(setMethod.IsStatic == false) {
            ilGen.
                Ldarg_0().
                UnboxOrCast(declaringType);
        }

        ilGen.
            Ldarg_1().
            Unbox_AnyOrCast(propertyType).
            CallOrVirt(setMethod).
            Ret();
        return (SetterDelegate)dynamicMethod.CreateDelegate(typeof(SetterDelegate));
    }

    /// <summary>
    ///     Creates a dynamic method to call a getter of a property info with minimum overhead
    /// </summary>
    public static GetterDelegate<TInstance, TProperty> CreateGetter<TInstance, TProperty>(PropertyInfo propertyInfo, bool allowNonPublic = true) {
        if(propertyInfo == null)
            throw new ArgumentNullException(nameof(propertyInfo));
        if((propertyInfo.CanRead == false) || propertyInfo.IsIndexerProperty())
            return null;

        var propertyName = propertyInfo.Name;
        var getMethod = propertyInfo.GetGetMethod(allowNonPublic);
        if(getMethod == null)
            return null;
        var declaringType = typeof(TInstance);
        var propertyType = typeof(TProperty);

        var dynamicMethod = new DynamicMethod($"DynamicGet{propertyName}", propertyType, new[] {declaringType},
            getMethod.Module, true);
        var ilGen = dynamicMethod.GetFluentIlGenerator();
        if(getMethod.IsStatic == false)
            ilGen.LdargIfClassElseLdarga(declaringType, 0);
        ilGen.
            LdargIfClassElseLdarga(declaringType, 0).
            CallOrVirt(getMethod).
            Ret();
        return (GetterDelegate<TInstance, TProperty>)
            dynamicMethod.CreateDelegate(typeof(GetterDelegate<TInstance, TProperty>));
    }

    /// <summary>
    ///     Creates a dynamic method to call a setter of a property info with minimum overhead
    /// </summary>
    public static SetterDelegate<TInstance, TProperty> CreateSetter<TInstance, TProperty>(PropertyInfo propertyInfo, bool allowNonPublic = true) {
        if(propertyInfo == null)
            throw new ArgumentNullException(nameof(propertyInfo));
        if((propertyInfo.CanWrite == false) || propertyInfo.IsIndexerProperty())
            return null;

        var propertyName = propertyInfo.Name;
        var setMethod = propertyInfo.GetSetMethod(allowNonPublic);
        if(setMethod == null)
            return null;
        var declaringType = propertyInfo.DeclaringType;
        var propertyType = propertyInfo.PropertyType;

        var dynamicMethod = new DynamicMethod($"DynamicSet{propertyName}", typeof(void),
            new[] {declaringType, propertyType}, propertyInfo.Module, true);
        var ilGen = dynamicMethod.GetFluentIlGenerator();
        if(setMethod.IsStatic == false)
            ilGen.LdargIfClassElseLdarga(declaringType, 0);
        ilGen.
            Ldarg_1().
            CallOrVirt(setMethod).
            Ret();
        return (SetterDelegate<TInstance, TProperty>)
            dynamicMethod.CreateDelegate(typeof(SetterDelegate<TInstance, TProperty>));
    }
    #endregion

    #region Fields
    /// <summary>
    ///     Creates a dynamic method to call a getter of a field info with minimum overhead
    /// </summary>
    public static GetterDelegate CreateGetter(FieldInfo fieldInfo, bool allowNonPublic = true) {
        if(fieldInfo == null)
            throw new ArgumentNullException(nameof(fieldInfo));
        if((fieldInfo.IsPublic == false) && (allowNonPublic == false))
            return null;
        var fieldName = fieldInfo.Name;
        var declaringType = fieldInfo.DeclaringType;
        var fieldType = fieldInfo.FieldType;

        var dynamicMethod = new DynamicMethod($"DynamicGet{fieldName}", typeof(object), new[] {typeof(object)},
            fieldInfo.Module, true);
        var ilGen = dynamicMethod.GetFluentIlGenerator();
        if(fieldInfo.IsLiteral) {
            if(fieldType == typeof(bool)) {
                if((bool)fieldInfo.GetRawConstantValue())
                    ilGen.Ldc_I4_1();
                else
                    ilGen.Ldc_I4_0();
            } else if(fieldType == typeof(int))
                ilGen.Ldc_I4_Auto((int)fieldInfo.GetRawConstantValue());
            else if(fieldType == typeof(float))
                ilGen.Ldc_R4((float)fieldInfo.GetRawConstantValue());
            else if(fieldType == typeof(double))
                ilGen.Ldc_R8((double)fieldInfo.GetRawConstantValue());
            else if(fieldType == typeof(string))
                ilGen.Ldstr((string)fieldInfo.GetRawConstantValue());
            else {
                throw new
                    NotSupportedException($"Creating a FieldGetter for type: {fieldType.Name} is unsupported.");
            }

            ilGen.Ret();
        } else if(fieldInfo.IsStatic) {
            ilGen.Ldsfld(fieldInfo).
                BoxOrCast(fieldType).
                Ret();
        } else {
            ilGen.
                Ldarg_0().
                UnboxOrCast(declaringType).
                Ldfld(fieldInfo).
                BoxOrCast(fieldType).
                Ret();
        }

        return (GetterDelegate)dynamicMethod.CreateDelegate(typeof(GetterDelegate));
    }

    /// <summary>
    ///     Creates a dynamic method to call a setter of a field info with minimum overhead
    /// </summary>
    public static SetterDelegate CreateSetter(FieldInfo fieldInfo, bool allowNonPublic = true) {
        if(fieldInfo == null)
            throw new ArgumentNullException(nameof(fieldInfo));
        if(fieldInfo.IsLiteral)
            return null;
        if((fieldInfo.IsInitOnly || fieldInfo.IsPrivate) && (allowNonPublic == false))
            return null;
        var fieldName = fieldInfo.Name;
        var declaringType = fieldInfo.DeclaringType;
        var fieldType = fieldInfo.FieldType;

        var dynamicMethod = new DynamicMethod($"DynamicSet{fieldName}", typeof(void),
            new[] {typeof(object), typeof(object)}, fieldInfo.Module, true);
        var ilGen = dynamicMethod.GetFluentIlGenerator();
        if(fieldInfo.IsStatic) {
            ilGen.
                Ldarg_1().
                Unbox_AnyOrCast(fieldType).
                Stsfld(fieldInfo).
                Ret();
        } else {
            ilGen.
                Ldarg_0().
                UnboxOrCast(declaringType).
                Ldarg_1().
                Unbox_AnyOrCast(fieldType).
                Stfld(fieldInfo).
                Ret();
        }

        return (SetterDelegate)dynamicMethod.CreateDelegate(typeof(SetterDelegate));
    }

    /// <summary>
    ///     Creates a dynamic method to call a getter of a field info with minimum overhead
    /// </summary>
    public static GetterDelegate<TInstance, TField> CreateGetter<TInstance, TField>(
        FieldInfo fieldInfo, bool allowNonPublic = true) {
        if(fieldInfo == null)
            throw new ArgumentNullException(nameof(fieldInfo));
        if(fieldInfo.IsPrivate && (allowNonPublic == false))
            return null;
        var fieldName = fieldInfo.Name;
        var declaringType = typeof(TInstance);
        var fieldType = typeof(TField);

        var dynamicMethod = new DynamicMethod($"DynamicGet{fieldName}", fieldType, new[] {declaringType},
            fieldInfo.Module, true);
        var ilGen = dynamicMethod.GetFluentIlGenerator();
        if(fieldInfo.IsLiteral) {
            if(fieldType == typeof(bool)) {
                if((bool)fieldInfo.GetRawConstantValue())
                    ilGen.Ldc_I4_1();
                else
                    ilGen.Ldc_I4_0();
            } else if(fieldType == typeof(int))
                ilGen.Ldc_I4_Auto((int)fieldInfo.GetRawConstantValue());
            else if(fieldType == typeof(float))
                ilGen.Ldc_R4((float)fieldInfo.GetRawConstantValue());
            else if(fieldType == typeof(double))
                ilGen.Ldc_R8((double)fieldInfo.GetRawConstantValue());
            else if(fieldType == typeof(string))
                ilGen.Ldstr((string)fieldInfo.GetRawConstantValue());
            else {
                throw new
                    NotSupportedException($"Creating a FieldGetter for type: {fieldType.Name} is unsupported.");
            }

            ilGen.Ret();
        } else if(fieldInfo.IsStatic) {
            ilGen.Ldsfld(fieldInfo).
                Ret();
        } else {
            ilGen.
                LdargIfClassElseLdarga(declaringType, 0).
                Ldfld(fieldInfo).
                Ret();
        }

        return (GetterDelegate<TInstance, TField>)
            dynamicMethod.CreateDelegate(typeof(GetterDelegate<TInstance, TField>));
    }

    /// <summary>
    ///     Creates a dynamic method to call a setter of a field info with minimum overhead
    /// </summary>
    public static SetterDelegate<TInstance, TField> CreateSetter<TInstance, TField>(
        FieldInfo fieldInfo, bool allowNonPublic = true) {
        if(fieldInfo == null)
            throw new ArgumentNullException(nameof(fieldInfo));
        if(fieldInfo.IsInitOnly || fieldInfo.IsLiteral)
            return null;
        if(fieldInfo.IsPrivate && (allowNonPublic == false))
            return null;
        var fieldName = fieldInfo.Name;
        var declaringType = fieldInfo.DeclaringType;
        var fieldType = fieldInfo.FieldType;

        var dynamicMethod = new DynamicMethod($"DynamicSet{fieldName}", typeof(void),
            new[] {declaringType, fieldType}, fieldInfo.Module, true);
        var ilGen = dynamicMethod.GetFluentIlGenerator();
        if(fieldInfo.IsStatic) {
            ilGen.
                Ldarg_1().
                Stsfld(fieldInfo).
                Ret();
        } else {
            ilGen.
                LdargIfClassElseLdarga(declaringType, 0).
                Ldarg_1().
                Stfld(fieldInfo).
                Ret();
        }

        return (SetterDelegate<TInstance, TField>)
            dynamicMethod.CreateDelegate(typeof(SetterDelegate<TInstance, TField>));
    }
    #endregion
}