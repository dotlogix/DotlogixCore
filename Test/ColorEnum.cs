// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ColorEnum.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using DotLogix.Core.Enumeration;
#endregion

namespace Test {
    [SupportsFlags]
    public sealed class ColorEnum : Enum<ColorEnum, ColorEnum.ColorEnumMember> {
        public static readonly ColorEnumMember Red = Instance.RegisterColor("Red", 1);
        public static readonly ColorEnumMember Blue = Instance.RegisterColor("Blue", 2);
        public static readonly ColorEnumMember Green = Instance.RegisterColor("Green", 4);

        public static ColorEnum Instance { get; } =
            Enums.Register<ColorEnum, ColorEnumMember>(new ColorEnum());

        private ColorEnumMember RegisterColor(string name, int value) {
            return Register(new ColorEnumMember(this, name, value));
        }

        public sealed class ColorEnumMember : EnumMember<ColorEnum, ColorEnumMember> {
            internal ColorEnumMember(ColorEnum definingColorEnum, string name, int value) : base(definingColorEnum, name, value) { }
        }
    }
}
