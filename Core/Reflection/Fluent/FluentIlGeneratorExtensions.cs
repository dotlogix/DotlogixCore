// ==================================================
// Copyright 2016(C) , DotLogix
// File:  FluentIlGeneratorExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
using System.Reflection;
using DotLogix.Core.Reflection.Fluent.Generator;
#endregion

namespace DotLogix.Core.Reflection.Fluent {
    public static class FluentIlGeneratorExtensions {
        #region Integer
        public static FluentIlGenerator Ldc_I4_Auto(this FluentIlGenerator fluentIlGenerator, int value) {
            switch(value) {
                case -1:
                    fluentIlGenerator.Ldc_I4_M1();
                    break;
                case 0:
                    fluentIlGenerator.Ldc_I4_0();
                    break;
                case 1:
                    fluentIlGenerator.Ldc_I4_1();
                    break;
                case 2:
                    fluentIlGenerator.Ldc_I4_2();
                    break;
                case 3:
                    fluentIlGenerator.Ldc_I4_3();
                    break;
                case 4:
                    fluentIlGenerator.Ldc_I4_4();
                    break;
                case 5:
                    fluentIlGenerator.Ldc_I4_5();
                    break;
                case 6:
                    fluentIlGenerator.Ldc_I4_6();
                    break;
                case 7:
                    fluentIlGenerator.Ldc_I4_7();
                    break;
                case 8:
                    fluentIlGenerator.Ldc_I4_8();
                    break;
                default:
                    if((value > -129) && (value < 128))
                        fluentIlGenerator.Ldc_I4_S((sbyte)value);
                    else
                        fluentIlGenerator.Ldc_I4(value);
                    break;
            }
            return fluentIlGenerator;
        }
        #endregion

        #region Arguments
        public static FluentIlGenerator Ldarg_Auto(this FluentIlGenerator fluentIlGenerator, short index) {
            switch(index) {
                case 0:
                    fluentIlGenerator.Ldarg_0();
                    break;
                case 1:
                    fluentIlGenerator.Ldarg_1();
                    break;
                case 2:
                    fluentIlGenerator.Ldarg_2();
                    break;
                case 3:
                    fluentIlGenerator.Ldarg_3();
                    break;
                default:
                    if((index > 0) && (index <= byte.MaxValue))
                        fluentIlGenerator.Ldarg_S((byte)index);
                    else
                        fluentIlGenerator.Ldarg(index);
                    break;
            }
            return fluentIlGenerator;
        }

        public static FluentIlGenerator Starg_Auto(this FluentIlGenerator fluentIlGenerator, short index) {
            if((index > 0) && (index <= byte.MaxValue))
                fluentIlGenerator.Starg_S((byte)index);
            else
                fluentIlGenerator.Starg(index);
            return fluentIlGenerator;
        }

        public static FluentIlGenerator Ldarga_Auto(this FluentIlGenerator fluentIlGenerator, short index) {
            if((index > 0) && (index <= byte.MaxValue))
                fluentIlGenerator.Ldarga_S((byte)index);
            else
                fluentIlGenerator.Ldarga(index);
            return fluentIlGenerator;
        }

        public static FluentIlGenerator LdargIfClassElseLdarga(this FluentIlGenerator fluentIlGenerator, Type type,
                                                               short index) {
            if(type.IsValueType)
                Ldarga_Auto(fluentIlGenerator, index);
            else
                Ldarg_Auto(fluentIlGenerator, index);
            return fluentIlGenerator;
        }
        #endregion

        #region Locals
        public static FluentIlGenerator Stloc_Auto(this FluentIlGenerator fluentIlGenerator, short index) {
            switch(index) {
                case 0:
                    fluentIlGenerator.Stloc_0();
                    break;
                case 1:
                    fluentIlGenerator.Stloc_1();
                    break;
                case 2:
                    fluentIlGenerator.Stloc_2();
                    break;
                case 3:
                    fluentIlGenerator.Stloc_3();
                    break;
                default:
                    if((index > 0) && (index <= byte.MaxValue))
                        fluentIlGenerator.Stloc_S((byte)index);
                    else
                        fluentIlGenerator.Stloc(index);
                    break;
            }
            return fluentIlGenerator;
        }

        public static FluentIlGenerator Ldloc_Auto(this FluentIlGenerator fluentIlGenerator, short index) {
            switch(index) {
                case 0:
                    fluentIlGenerator.Ldloc_0();
                    break;
                case 1:
                    fluentIlGenerator.Ldloc_1();
                    break;
                case 2:
                    fluentIlGenerator.Ldloc_2();
                    break;
                case 3:
                    fluentIlGenerator.Ldloc_3();
                    break;
                default:
                    if((index > 0) && (index <= byte.MaxValue))
                        fluentIlGenerator.Ldloc_S((byte)index);
                    else
                        fluentIlGenerator.Ldloc(index);
                    break;
            }
            return fluentIlGenerator;
        }

        public static FluentIlGenerator Ldloca_Auto(this FluentIlGenerator fluentIlGenerator, short index) {
            if((index > 0) && (index <= byte.MaxValue))
                fluentIlGenerator.Ldloca_S((byte)index);
            else
                fluentIlGenerator.Ldloca(index);
            return fluentIlGenerator;
        }
        #endregion

        #region InvokeDelegate
        public static FluentIlGenerator CallOrVirt(this FluentIlGenerator fluentIlGenerator, MethodInfo methodInfo) {
            if(methodInfo.IsVirtual)
                fluentIlGenerator.Callvirt(methodInfo);
            else
                fluentIlGenerator.Call(methodInfo);
            return fluentIlGenerator;
        }

        public static FluentIlGenerator CallOrVirt(this FluentIlGenerator fluentIlGenerator, MethodInfo methodInfo,
                                                   Type[] optionalParameterTypes) {
            if(methodInfo.IsVirtual)
                fluentIlGenerator.Callvirt(methodInfo, optionalParameterTypes);
            else
                fluentIlGenerator.Call(methodInfo, optionalParameterTypes);
            return fluentIlGenerator;
        }
        #endregion

        #region Boxing
        public static FluentIlGenerator UnboxOrCast(this FluentIlGenerator fluentIlGenerator, Type type) {
            if(type.IsValueType)
                fluentIlGenerator.Unbox(type);
            else
                fluentIlGenerator.Castclass(type);
            return fluentIlGenerator;
        }

        public static FluentIlGenerator Unbox_AnyOrCast(this FluentIlGenerator fluentIlGenerator, Type type) {
            if(type.IsValueType)
                fluentIlGenerator.Unbox_Any(type);
            else
                fluentIlGenerator.Castclass(type);
            return fluentIlGenerator;
        }

        public static FluentIlGenerator BoxIfNeeded(this FluentIlGenerator fluentIlGenerator, Type type) {
            if(type.IsValueType)
                fluentIlGenerator.Box(type);
            return fluentIlGenerator;
        }


        public static FluentIlGenerator BoxOrCast(this FluentIlGenerator fluentIlGenerator, Type type) {
            if(type.IsValueType)
                fluentIlGenerator.Box(type);
            else
                fluentIlGenerator.Castclass(type);
            return fluentIlGenerator;
        }
        #endregion
    }
}
