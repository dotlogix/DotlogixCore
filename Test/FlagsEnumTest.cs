// ==================================================
// Copyright 2018(C) , DotLogix
// File:  FlagsEnumTest.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using DotLogix.Core.Enumeration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ColorFlags = DotLogix.Core.Enumeration.EnumFlags<Test.ColorEnum, Test.ColorEnum.ColorEnumMember>;
#endregion

namespace Test {
    [TestClass]
    public class FlagsEnumTest {
        [TestMethod]
        public void Converters() {
            var blue = ColorEnum.Blue;
            ColorFlags blueFlags = blue;
            Assert.AreEqual(blue.Name, blueFlags.Name);
            Assert.AreEqual(blue.Value, blueFlags.Value);
            var blueBack = (ColorEnum.ColorEnumMember)blueFlags;
            Assert.AreEqual(blue, blueBack);
            Assert.AreEqual(blue.Name, blueBack.Name);
            Assert.AreEqual(blue.Value, blueBack.Value);
        }

        [TestMethod]
        public void EqualityTest() {
            var blueRed = new ColorFlags(ColorEnum.Instance, ColorEnum.Blue, ColorEnum.Red);
            var redBlue = new ColorFlags(ColorEnum.Instance, ColorEnum.Red, ColorEnum.Blue);

            Assert.AreEqual(blueRed, redBlue);
        }

        [TestMethod]
        public void OrTest() {
            ColorEnum.Blue.ToFlag<ColorEnum, ColorEnum.ColorEnumMember>();

            var blueRed = new ColorFlags(ColorEnum.Instance, ColorEnum.Blue, ColorEnum.Red);
            var red = new ColorFlags(ColorEnum.Instance, ColorEnum.Red);
            var blue = new ColorFlags(ColorEnum.Instance, ColorEnum.Blue);
            Assert.AreEqual(blueRed, red | blue);
        }

        [TestMethod]
        public void NotTest() {
            var all = new ColorFlags(ColorEnum.Instance, ColorEnum.Instance.Members);
            var none = new ColorFlags(ColorEnum.Instance);
            var green = new ColorFlags(ColorEnum.Instance, ColorEnum.Green);
            var redBlue = new ColorFlags(ColorEnum.Instance, ColorEnum.Red, ColorEnum.Blue);

            Assert.AreEqual(all, ~none);
            Assert.AreEqual(redBlue, ~green);
        }

        [TestMethod]
        public void AndTest() {
            var all = new ColorFlags(ColorEnum.Instance, ColorEnum.Instance.Members);
            var none = new ColorFlags(ColorEnum.Instance);

            var blue = new ColorFlags(ColorEnum.Instance, ColorEnum.Blue);
            var greenBlue = new ColorFlags(ColorEnum.Instance, ColorEnum.Green, ColorEnum.Blue);
            var redBlue = new ColorFlags(ColorEnum.Instance, ColorEnum.Red, ColorEnum.Blue);

            Assert.AreEqual(greenBlue, all & greenBlue);
            Assert.AreEqual(none, none & greenBlue);
            Assert.AreEqual(blue, greenBlue & redBlue);
        }

        [TestMethod]
        public void XOrTest() {
            var all = new ColorFlags(ColorEnum.Instance, ColorEnum.Instance.Members);
            var none = new ColorFlags(ColorEnum.Instance);

            var red = new ColorFlags(ColorEnum.Instance, ColorEnum.Red);
            var redGreen = new ColorFlags(ColorEnum.Instance, ColorEnum.Red, ColorEnum.Green);
            var greenBlue = new ColorFlags(ColorEnum.Instance, ColorEnum.Green, ColorEnum.Blue);
            var redBlue = new ColorFlags(ColorEnum.Instance, ColorEnum.Red, ColorEnum.Blue);

            Assert.AreEqual(red, all ^ greenBlue);
            Assert.AreEqual(greenBlue, none ^ greenBlue);
            Assert.AreEqual(redGreen, greenBlue ^ redBlue);
        }
    }
}
