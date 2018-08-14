// ==================================================
// Copyright 2018(C) , DotLogix
// File:  FluentIlExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Reflection;
using System.Reflection.Emit;
using DotLogix.Core.Reflection.Fluent.Generator;
#endregion

namespace DotLogix.Core.Reflection.Fluent {
    public static class FluentIlExtensions {
        public static FluentIlGenerator AsFluent(this ILGenerator ilGenerator) {
            return new FluentIlGenerator(ilGenerator);
        }

        public static FluentIlGenerator GetFluentIlGenerator(this DynamicMethod dynamicMethod) {
            return new FluentIlGenerator(dynamicMethod.GetILGenerator());
        }

        public static FluentIlGenerator GetFluentIlGenerator(this ConstructorBuilder constructorBuilder) {
            return new FluentIlGenerator(constructorBuilder.GetILGenerator());
        }

        public static FluentIlGenerator GetFluentIlGenerator(this MethodBuilder methodBuilder) {
            return new FluentIlGenerator(methodBuilder.GetILGenerator());
        }

        public static FluentIlGenerator GetFluentIlGenerator(this DynamicMethod dynamicMethod, int streamSize) {
            return new FluentIlGenerator(dynamicMethod.GetILGenerator(streamSize));
        }

        public static FluentIlGenerator
        GetFluentIlGenerator(this ConstructorBuilder constructorBuilder, int streamSize) {
            return new FluentIlGenerator(constructorBuilder.GetILGenerator(streamSize));
        }

        public static FluentIlGenerator GetFluentIlGenerator(this MethodBuilder methodBuilder, int streamSize) {
            return new FluentIlGenerator(methodBuilder.GetILGenerator(streamSize));
        }

        public static bool IsIndexerProperty(this PropertyInfo propertyInfo) {
            return propertyInfo.GetIndexParameters().Length > 0;
        }
    }
}
