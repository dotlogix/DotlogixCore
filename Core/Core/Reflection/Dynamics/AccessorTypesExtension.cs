// ==================================================
// Copyright 2018(C) , DotLogix
// File:  AccessorTypesExtension.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Reflection;
#endregion

namespace DotLogix.Core.Reflection.Dynamics {
    /// <summary>
    /// A static class providing extension methods for <see cref="AccessorTypes"/>
    /// </summary>
    public static class AccessorTypesExtension {
        /// <summary>
        /// Get the internal representation of member types
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static MemberTypes GetMemberTypes(this AccessorTypes accessorTypes) {
            MemberTypes memberTypes;
            switch(accessorTypes) {
                case AccessorTypes.None:
                    memberTypes = 0;
                    break;
                case AccessorTypes.Property:
                    memberTypes = MemberTypes.Property;
                    break;
                case AccessorTypes.Field:
                    memberTypes = MemberTypes.Field;
                    break;
                case AccessorTypes.Any:
                    memberTypes = MemberTypes.Property | MemberTypes.Field;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(accessorTypes), accessorTypes, null);
            }
            return memberTypes;
        }
    }
}
