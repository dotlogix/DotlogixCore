using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Expressions {
    public static partial class Lambdas {
        public static Lambda<TResult> Call<TResult>(this Lambda instance, string methodName, params Lambda[] args) {
            return Call<TResult>(instance, methodName, args.AsEnumerable());
        }

        public static Lambda<TResult> Call<TResult>(this Lambda instance, string methodName, IEnumerable<Lambda> args) {
            args = args.AsReadOnlyCollection();
            return Call<TResult>(instance, ResolveMethod(instance, methodName, args), args);
        }

        public static Lambda<TResult> Call<TResult>(this Lambda instance, MethodInfo methodInfo, params Lambda[] args) {
            return Call<TResult>(instance, methodInfo, args.AsReadOnlyCollection());
        }

        public static Lambda<TResult> Call<TResult>(this Lambda instance, MethodInfo methodInfo, IEnumerable<Lambda> args) {
            if(methodInfo.ReturnType != typeof(TResult)) {
                throw new ArgumentException($"Expected method return type {typeof(TResult).Name}, but got {methodInfo.ReturnType.Name}");
            }

            var parameters = methodInfo.GetParameters();
            var arguments = new Expression[parameters.Length];
            var idx = 0;
            foreach(var arg in args) {
                var parameterType = parameters[idx].ParameterType;
                var argument = arg.Cast(parameterType);
                arguments[idx] = argument.Body;
                idx++;
            }

            if(idx < parameters.Length && parameters[idx].IsOptional == false) {
                throw new ArgumentException($"Not enough parameters. Expected {parameters.Length}, but got {idx}");
            }

            return Expression.Call(instance.Body, methodInfo, args.Select(a => a.Body).ToArray());
        }

        public static Lambda<TResult> Call<TResult>(this Lambda instance, Func<TResult> method) {
            return Call<TResult>(instance, method.Method);
        }

        public static Lambda<TResult> Call<T1, TResult>(this Lambda instance, Func<T1, TResult> method, Lambda<T1> arg1) {
            return Call<TResult>(instance, method.Method, arg1);
        }

        public static Lambda<TResult> Call<T1, T2, TResult>(this Lambda instance, Func<T1, T2, TResult> method, Lambda<T1> arg1, Lambda<T2> arg2) {
            return Call<TResult>(instance, method.Method, arg1, arg2);
        }

        public static Lambda<TResult> Call<T1, T2, T3, TResult>(this Lambda instance, Func<T1, T2, T3, TResult> method, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3) {
            return Call<TResult>(instance, method.Method, arg1, arg2, arg3);
        }

        public static Lambda<TResult> Call<T1, T2, T3, T4, TResult>(this Lambda instance, Func<T1, T2, T3, T4, TResult> method, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4) {
            return Call<TResult>(instance, method.Method, arg1, arg2, arg3, arg4);
        }

        public static Lambda<TResult> Call<T1, T2, T3, T4, T5, TResult>(this Lambda instance, Func<T1, T2, T3, T4, T5, TResult> method, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5) {
            return Call<TResult>(instance, method.Method, arg1, arg2, arg3, arg4, arg5);
        }

        public static Lambda<TResult> Call<T1, T2, T3, T4, T5, T6, TResult>(this Lambda instance, Func<T1, T2, T3, T4, T5, T6, TResult> method, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5, Lambda<T6> arg6) {
            return Call<TResult>(instance, method.Method, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        public static Lambda<TResult> Call<T1, T2, T3, T4, T5, T6, T7, TResult>(this Lambda instance, Func<T1, T2, T3, T4, T5, T6, T7, TResult> method, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5, Lambda<T6> arg6, Lambda<T7> arg7) {
            return Call<TResult>(instance, method.Method, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        public static Lambda<TResult> Call<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Lambda instance, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> method, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5, Lambda<T6> arg6, Lambda<T7> arg7, Lambda<T8> arg8) {
            return Call<TResult>(instance, method.Method, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        public static Lambda<TResult> Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Lambda instance, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> method, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5, Lambda<T6> arg6, Lambda<T7> arg7, Lambda<T8> arg8, Lambda<T9> arg9) {
            return Call<TResult>(instance, method.Method, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        public static Lambda<TResult> Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Lambda instance, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> method, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5, Lambda<T6> arg6, Lambda<T7> arg7, Lambda<T8> arg8, Lambda<T9> arg9, Lambda<T10> arg10) {
            return Call<TResult>(instance, method.Method, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        public static Lambda<TResult> CallStatic<T, TResult>(this Lambda<T> instance, Func<T, TResult> method) {
            return CallStatic<TResult>(method.Method, instance);
        }

        public static Lambda<TResult> CallStatic<T, T1, TResult>(this Lambda<T> instance, Func<T, T1, TResult> method, Lambda<T1> arg1) {
            return CallStatic<TResult>(method.Method, instance, arg1);
        }

        public static Lambda<TResult> CallStatic<T, T1, T2, TResult>(this Lambda<T> instance, Func<T, T1, T2, TResult> method, Lambda<T1> arg1, Lambda<T2> arg2) {
            return CallStatic<TResult>(method.Method, instance, arg1, arg2);
        }

        public static Lambda<TResult> CallStatic<T, T1, T2, T3, TResult>(this Lambda<T> instance, Func<T, T1, T2, T3, TResult> method, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3) {
            return CallStatic<TResult>(method.Method, instance, arg1, arg2, arg3);
        }

        public static Lambda<TResult> CallStatic<T, T1, T2, T3, T4, TResult>(this Lambda<T> instance, Func<T, T1, T2, T3, T4, TResult> method, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4) {
            return CallStatic<TResult>(method.Method, instance, arg1, arg2, arg3, arg4);
        }

        public static Lambda<TResult> CallStatic<T, T1, T2, T3, T4, T5, TResult>(this Lambda<T> instance, Func<T, T1, T2, T3, T4, T5, TResult> method, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5) {
            return CallStatic<TResult>(method.Method, instance, arg1, arg2, arg3, arg4, arg5);
        }

        public static Lambda<TResult> CallStatic<T, T1, T2, T3, T4, T5, T6, TResult>(this Lambda<T> instance, Func<T, T1, T2, T3, T4, T5, T6, TResult> method, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5, Lambda<T6> arg6) {
            return CallStatic<TResult>(method.Method, instance, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        public static Lambda<TResult> CallStatic<T, T1, T2, T3, T4, T5, T6, T7, TResult>(this Lambda<T> instance, Func<T, T1, T2, T3, T4, T5, T6, T7, TResult> method, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5, Lambda<T6> arg6, Lambda<T7> arg7) {
            return CallStatic<TResult>(method.Method, instance, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        public static Lambda<TResult> CallStatic<T, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Lambda<T> instance, Func<T, T1, T2, T3, T4, T5, T6, T7, T8, TResult> method, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5, Lambda<T6> arg6, Lambda<T7> arg7, Lambda<T8> arg8) {
            return CallStatic<TResult>(method.Method, instance, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        public static Lambda<TResult> CallStatic<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Lambda<T> instance, Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> method, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5, Lambda<T6> arg6, Lambda<T7> arg7, Lambda<T8> arg8, Lambda<T9> arg9) {
            return CallStatic<TResult>(method.Method, instance, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        public static Lambda<TResult> CallStatic<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Lambda<T> instance, Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> method, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5, Lambda<T6> arg6, Lambda<T7> arg7, Lambda<T8> arg8, Lambda<T9> arg9, Lambda<T10> arg10) {
            return CallStatic<TResult>(method.Method, instance, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        public static Lambda<TResult> CallStatic<TResult>(Type type, string methodName, params Lambda[] args) {
            return CallStatic<TResult>(type, methodName, args.AsEnumerable());
        }

        public static Lambda<TResult> CallStatic<TResult>(Type type, string methodName, IEnumerable<Lambda> args) {
            args = args.AsReadOnlyCollection();
            return CallStatic<TResult>(ResolveStaticMethod(type, methodName, args), args);
        }

        public static Lambda<TResult> CallStatic<TResult>(MethodInfo methodInfo, params Lambda[] args) {
            return CallStatic<TResult>(methodInfo, args.AsEnumerable());
        }

        public static Lambda<TResult> CallStatic<TResult>(MethodInfo methodInfo, IEnumerable<Lambda> args) {
            if(methodInfo.ReturnType != typeof(TResult)) {
                throw new ArgumentException($"Expected method return type {typeof(TResult).Name}, but got {methodInfo.ReturnType.Name}");
            }

            var parameters = methodInfo.GetParameters();
            var arguments = new Expression[parameters.Length];
            var idx = 0;
            foreach(var arg in args) {
                var parameterType = parameters[idx].ParameterType;
                var argument = arg.Cast(parameterType);
                arguments[idx] = argument.Body;
                idx++;
            }

            if(idx < parameters.Length && parameters[idx].IsOptional == false) {
                throw new ArgumentException($"Not enough parameters. Expected {parameters.Length}, but got {idx}");
            }

            return Expression.Call(methodInfo, args.Select(a => a.Body).ToArray());
        }

        public static Lambda<TResult> CallStatic<TResult>(Func<TResult> methodFunc) {
            return CallStatic<TResult>(methodFunc.Method);
        }

        public static Lambda<TResult> CallStatic<T1, TResult>(Func<T1, TResult> methodFunc, Lambda<T1> arg1) {
            return CallStatic<TResult>(methodFunc.Method, arg1);
        }

        public static Lambda<TResult> CallStatic<T1, T2, TResult>(Func<T1, T2, TResult> methodFunc, Lambda<T1> arg1, Lambda<T2> arg2) {
            return CallStatic<TResult>(methodFunc.Method, arg1, arg2);
        }

        public static Lambda<TResult> CallStatic<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> methodFunc, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3) {
            return CallStatic<TResult>(methodFunc.Method, arg1, arg2, arg3);
        }

        public static Lambda<TResult> CallStatic<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> methodFunc, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4) {
            return CallStatic<TResult>(methodFunc.Method, arg1, arg2, arg3, arg4);
        }

        public static Lambda<TResult> CallStatic<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> methodFunc, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5) {
            return CallStatic<TResult>(methodFunc.Method, arg1, arg2, arg3, arg4, arg5);
        }

        public static Lambda<TResult> CallStatic<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> methodFunc, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5, Lambda<T6> arg6) {
            return CallStatic<TResult>(methodFunc.Method, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        public static Lambda<TResult> CallStatic<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> methodFunc, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5, Lambda<T6> arg6, Lambda<T7> arg7) {
            return CallStatic<TResult>(methodFunc.Method, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        public static Lambda<TResult> CallStatic<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> methodFunc, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5, Lambda<T6> arg6, Lambda<T7> arg7, Lambda<T8> arg8) {
            return CallStatic<TResult>(methodFunc.Method, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        public static Lambda<TResult> CallStatic<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> methodFunc, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5, Lambda<T6> arg6, Lambda<T7> arg7, Lambda<T8> arg8, Lambda<T9> arg9) {
            return CallStatic<TResult>(methodFunc.Method, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        public static Lambda<TResult> CallStatic<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> methodFunc, Lambda<T1> arg1, Lambda<T2> arg2, Lambda<T3> arg3, Lambda<T4> arg4, Lambda<T5> arg5, Lambda<T6> arg6, Lambda<T7> arg7, Lambda<T8> arg8, Lambda<T9> arg9, Lambda<T10> arg10) {
            return CallStatic<TResult>(methodFunc.Method, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        #region Helpers
   
        private static MethodInfo ResolveMethod(Lambda instance, string methodName, IEnumerable<Lambda> lambdaBuilders) {
            var types = lambdaBuilders.Select(b => b.Type).ToArray();
#if NETSTANDARD2_1
            return instance.Type.GetMethod(methodName, 0, BindingFlags.Instance, default, CallingConventions.Any, types, default);
#else
            return instance.Type.GetMethod(methodName, BindingFlags.Instance, default, CallingConventions.Any, types, default);
#endif
        }

        private static MethodInfo ResolveStaticMethod(Type type, string methodName, IEnumerable<Lambda> lambdaBuilders) {
            var types = lambdaBuilders.Select(b => b.Type).ToArray();
#if NETSTANDARD2_1
            return type.GetMethod(methodName, 0, BindingFlags.Static, default, CallingConventions.Any, types, default);
#else
            return type.GetMethod(methodName, BindingFlags.Static, default, CallingConventions.Any, types, default);
#endif
        }

        #endregion
    }
}