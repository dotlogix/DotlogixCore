// ==================================================
// Copyright 2018(C) , DotLogix
// File:  TypeExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Text;
#endregion

namespace DotLogix.Core.Extensions {
    public static class TypeExtensions {
        public static string ReadableName(this Type type) {
            var sb = new StringBuilder();
            BuildName(sb, type);
            return sb.ToString();
        }

        private static void BuildName(StringBuilder sb, Type type) {
            if(!type.IsGenericType)
                sb.Append(type.Name);
            else {
                var name = type.Name.Split("`".ToCharArray())[0];
                sb.Append(name);
                sb.Append("<");
                foreach(var param in type.GetGenericArguments()) {
                    BuildName(sb, param);
                    sb.Append(",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(">");
            }
        }
    }
}
