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
    /// <summary>
    /// A representation of a constructor
    /// </summary>
    public class DynamicCtor {
        /// <summary>
        /// The original constructor info
        /// </summary>
        public ConstructorInfo ConstructorInfo { get; }

        /// <summary>
        /// The access modifiers
        /// </summary>
        public AccessModifiers Access { get; }

        /// <summary>
        /// The visibility modifiers
        /// </summary>
        public VisibilityModifiers Visibility { get; }

        /// <summary>
        /// The constructor delegate
        /// </summary>
        public CtorDelegate CtorDelegate { get; }

        /// <summary>
        /// The declaring type
        /// </summary>
        public Type DeclaringType { get; }

        /// <summary>
        /// The parameters
        /// </summary>
        public ParameterInfo[] Parameters { get; }
        /// <summary>
        /// The parameter types
        /// </summary>
        public Type[] ParameterTypes { get; }

        /// <summary>
        /// The parameter count
        /// </summary>
        public int ParameterCount => Parameters.Length;

        /// <summary>
        /// The constructor does not require arguments
        /// </summary>
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

        /// <summary>
        /// Invoke the constructor and create an instance
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
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

        /// <summary>
        /// Returns a representation of the constructor with its parameter type names
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return $"Ctor<{DeclaringType.Name}>({string.Join(", ", ParameterTypes.Select(t => t.Name))})";
        }
    }
}
