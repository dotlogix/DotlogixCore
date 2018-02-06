// ==================================================
// Copyright 2016(C) , DotLogix
// File:  DynamicInvoke.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
using System.Linq;
using System.Reflection;
using DotLogix.Core.Reflection.Delegates;
#endregion

namespace DotLogix.Core.Reflection.Dynamics {
    public class DynamicInvoke {
        public MethodInfo MethodInfo { get; }
        public AccessModifiers Access { get; }
        public VisibilityModifiers Visibility { get; }
        public InvokeDelegate InvokeDelegate { get; }

        public Type DeclaringType { get; }
        public Type ReturnType { get; }
        public ParameterInfo[] Parameters { get; }
        public Type[] ParameterTypes { get; }
        public int ParameterCount => Parameters.Length;
        public string Name { get; set; }

        internal DynamicInvoke(MethodInfo methodInfo, AccessModifiers access, VisibilityModifiers visibility,
                               InvokeDelegate invokeDelegate) {
            MethodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
            InvokeDelegate = invokeDelegate ?? throw new ArgumentNullException(nameof(invokeDelegate));
            Name = methodInfo.Name;
            Access = access;
            Visibility = visibility;
            DeclaringType = methodInfo.DeclaringType;
            Parameters = methodInfo.GetParameters();
            ParameterTypes = Parameters.Select(p => p.ParameterType).ToArray();
            ReturnType = methodInfo.ReturnType;
        }

        public object StaticInvoke(params object[] parameters) {
            return Invoke(null, parameters);
        }

        public object Invoke(object instance, params object[] parameters) {
            if((instance == null) && ((Access & AccessModifiers.Static) == 0))
                throw new ArgumentNullException(nameof(instance), "Can not read value without an instance");

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

        public override string ToString() {
            return $"{DeclaringType.Name}.{Name}({string.Join(", ", ParameterTypes.Select(t => t.Name))})";
        }
    }
}
