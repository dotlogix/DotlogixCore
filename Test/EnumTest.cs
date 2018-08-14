// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EnumTest.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Core.Enumeration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endregion

namespace Test {
    [TestClass]
    public class EnumTest {
        [TestMethod]
        public void EqualityTest() {
            Assert.AreNotEqual(ColorEnum.Red, ColorEnum.Blue);
        }

        [TestMethod]
        public void ParseTest() {
            var redColor = Enums.Parse("ColorEnum.Red");
            Assert.AreEqual(ColorEnum.Red, redColor);

            redColor = Enums.Parse<ColorEnum.ColorEnumMember>("Red");
            Assert.AreEqual(ColorEnum.Red, redColor);

            redColor = Enums.Parse<ColorEnum.ColorEnumMember>(1);
            Assert.AreEqual(ColorEnum.Red, redColor);

            Assert.AreEqual("ColorEnum.Red", redColor.ToString());
        }
    }
}
