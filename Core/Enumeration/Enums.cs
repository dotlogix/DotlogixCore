// ==================================================
// Copyright 2016(C) , DotLogix
// File:  Enums.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  10.07.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Enumeration {
    public static class Enums {
        private static readonly Dictionary<Type, IEnum> TypeMapping = new Dictionary<Type, IEnum>();
        private static readonly Dictionary<Type, IEnum> MemberTypeMapping = new Dictionary<Type, IEnum>();
        private static readonly Dictionary<string, IEnum> NameMapping = new Dictionary<string, IEnum>();

        public static TEnum Register<TEnum, TEnumMember>(TEnum singletonInstance)
            where TEnum : class, IEnum<TEnum, TEnumMember>
            where TEnumMember : class, IEnumMember<TEnum, TEnumMember> {
            TypeMapping.Add(typeof(TEnum), singletonInstance);
            MemberTypeMapping.Add(typeof(TEnumMember), singletonInstance);
            NameMapping.Add(singletonInstance.Name, singletonInstance);
            return singletonInstance;
        }

        public static IEnumMember Parse(Type enumType, int value) {
            return GetEnum(enumType)?.GetByValue(value);
        }

        public static IEnumMember Parse(Type enumType, string value) {
            return GetEnum(enumType)?.GetByName(value);
        }

        public static TEnumMember Parse<TEnumMember>(int value) where TEnumMember : class, IEnumMember {
            var enu = GetEnumByMemberType<TEnumMember>();
            return enu?.GetByValue(value) as TEnumMember;
        }

        public static TEnumMember Parse<TEnumMember>(string value) where TEnumMember : class, IEnumMember {
            var enu = GetEnumByMemberType<TEnumMember>();
            if(enu == null)
                return null;

            var split = value.Split('.');
            switch(split.Length) {
                case 1:
                    return enu.GetByName(split[0]) as TEnumMember;
                case 2:
                    return enu.GetByName(split[1]) as TEnumMember;
                default:
                    return null;
            }
        }

        public static IEnumMember Parse(string value) {
            var split = value.Split('.');
            if(split.Length != 2)
                return null;
            var enu = GetEnumByName(split[0]);
            return enu?.GetByName(split[1]);
        }


        public static TEnum GetEnum<TEnum>() where TEnum : class, IEnum {
            return GetEnum(typeof(TEnum)) as TEnum;
        }

        public static IEnum GetEnum(Type enumType) {
            return TypeMapping.TryGetValue(enumType, out var value) ? value : null;
        }

        public static IEnum GetEnumByMemberType<TEnumMember>() where TEnumMember : class, IEnumMember {
            return GetEnumByMemberType(typeof(TEnumMember));
        }

        public static IEnum GetEnumByMemberType(Type memberType) {
            return MemberTypeMapping.TryGetValue(memberType, out var value) ? value : null;
        }

        public static IEnum GetEnumByName(string name) {
            return NameMapping.TryGetValue(name, out var value) ? value : null;
        }
    }
}
