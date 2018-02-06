using DotLogix.Core.Enumeration;

namespace Test {
    [SupportsFlags]
    public sealed class ColorEnum : Enum<ColorEnum, ColorEnum.ColorEnumMember>
    {
        public static ColorEnum Instance { get; } =
            Enums.Register<ColorEnum, ColorEnumMember>(new ColorEnum());

        public static ColorEnumMember Red = Instance.RegisterColor("Red", 1);
        public static ColorEnumMember Blue = Instance.RegisterColor("Blue", 2);
        public static ColorEnumMember Green = Instance.RegisterColor("Green", 4);

        public sealed class ColorEnumMember : EnumMember<ColorEnum, ColorEnumMember>
        {
            internal ColorEnumMember(ColorEnum definingColorEnum, string name, int value) : base(definingColorEnum, name, value) { }
        }

        private ColorEnumMember RegisterColor(string name, int value)
        {
            return Register(new ColorEnumMember(this, name, value));
        }
    }
}