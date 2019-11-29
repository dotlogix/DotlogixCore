// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DynamicInvoke.cs
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
    /// A representation of a method
    /// </summary>
    public class DynamicInvoke  : DynamicMember{
        /// <summary>
        /// The access modifiers
        /// </summary>
        public AccessModifiers Access { get; }
        /// <summary>
        /// The visibility modifiers
        /// </summary>
        public VisibilityModifiers Visibility { get; }
        /// <summary>
        /// The original method info
        /// </summary>
        public MethodInfo MethodInfo { get; }

        /// <summary>
        /// The invocation delegate
        /// </summary>
        public InvokeDelegate InvokeDelegate { get; }
        /// <summary>
        /// The return type
        /// </summary>
        public Type ReturnType { get; }
        /// <summary>
        /// The parameter infos
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
        internal DynamicInvoke(MethodInfo methodInfo, AccessModifiers access, VisibilityModifiers visibility,
                               InvokeDelegate invokeDelegate) : base(methodInfo) {
            MethodInfo = methodInfo;
            InvokeDelegate = invokeDelegate ?? throw new ArgumentNullException(nameof(invokeDelegate));
            Access = access;
            Visibility = visibility;
            Parameters = methodInfo.GetParameters();
            ParameterTypes = Parameters.Select(p => p.ParameterType).ToArray();
            ReturnType = methodInfo.ReturnType;
        }

        /// <summary>
        /// Invoke the delegate with the provided parameters.<br></br>
        /// The method must be static
        /// </summary>
        /// <returns></returns>
        public object StaticInvoke(params object[] parameters) {
            return Invoke(null, parameters);
        }

        /// <summary>
        /// Invoke the delegate with the provided parameters.<br></br>
        /// The method must be static
        /// </summary>
        /// <returns></returns>
        public object Invoke(object instance, params object[] parameters) {
            if((instance == null) && ((Access & AccessModifiers.Static) == 0))
                throw new ArgumentNullException(nameof(instance), "Can not read value without a new instance");

            var parameterCount = ParameterCount;
            if(parameters.Length >= parameterCount)
                return InvokeDelegate.Invoke(instance, parameters);

            var oldParameters = parameters;
            parameters = new object[parameterCount];
            for(var i = oldParameters.Length + 1; i < parameterCount; i++) {
                var parameterInfo = Parameters[i];
                if(parameterInfo.IsOptional == false)
                    throw new ArgumentException("Not enough parameters", nameof(parameters));
                parameters[i] = parameterInfo.DefaultValue;
            }
            oldParameters.CopyTo(parameters, 0);
            return InvokeDelegate.Invoke(instance, parameters);
        }

        /// <summary>
        /// Returns a string representation of the method with its parameters
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return $"{DeclaringType.Name}.{Name}({string.Join(", ", ParameterTypes.Select(t => t.Name))})";
        }
    }
}
