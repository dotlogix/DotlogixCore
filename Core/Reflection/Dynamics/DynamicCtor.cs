// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DynamicCtor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Linq;
using System.Reflection;
using DotLogix.Core.Reflection.Delegates;
#endregion

namespace DotLogix.Core.Reflection.Dynamics {
    public class DynamicCtor {
        public ConstructorInfo ConstructorInfo { get; }
        public AccessModifiers Access { get; }
        public VisibilityModifiers Visibility { get; }
        public CtorDelegate CtorDelegate { get; }
        public Type DeclaringType { get; }
        public ParameterInfo[] Parameters { get; }
        public Type[] ParameterTypes { get; }
        public int ParameterCount => Parameters.Length;
        public bool IsDefault => ParameterCount == 0;

        internal DynamicCtor(Type declaringType, ConstructorInfo constructorInfo, AccessModifiers access,
                             VisibilityModifiers visibility,
                             CtorDelegate ctorDelegate) {
            DeclaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));
            CtorDelegate = ctorDelegate ?? throw new ArgumentNullException(nameof(ctorDelegate));
            ConstructorInfo = constructorInfo;
            Access = access;
            Visibility = visibility;
            if(constructorInfo == null) {
                Parameters = new ParameterInfo[0];
                ParameterTypes = Type.EmptyTypes;
            } else {
                Parameters = constructorInfo.GetParameters();
                ParameterTypes = Parameters.Select(p => p.ParameterType).ToArray();
            }
        }

        public object Invoke(params object[] parameters) {
            var parameterCount = ParameterCount;
            if(parameters.Length >= parameterCount)
                return CtorDelegate.Invoke(parameters);

            var oldParameters = parameters;
            parameters = new object[parameterCount];
            for(var i = oldParameters.Length + 1; i < parameterCount; i++) {
                var parameterInfo = Parameters[i];
                if(parameterInfo.IsOptional == false)
                    throw new ArgumentException("Not enough parameters", nameof(parameters));
                parameters[i] = parameterInfo.DefaultValue;
            }
            oldParameters.CopyTo(parameters, 0);
            return CtorDelegate.Invoke(parameters);
        }

        public override string ToString() {
            return $"Ctor<{DeclaringType.Name}>({string.Join(", ", ParameterTypes.Select(t => t.Name))})";
        }
    }
}
