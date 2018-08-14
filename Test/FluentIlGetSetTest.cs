// ==================================================
// Copyright 2018(C) , DotLogix
// File:  FluentIlGetSetTest.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Core.Reflection.Fluent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endregion

namespace Test {
    [TestClass]
    public class FluentIlGetSetTest {
        [TestMethod]
        public void DynamicGetterSetterFieldTest() {
            var testType = typeof(FluentIlTestClass);
            var testInstance = new FluentIlTestClass();

            var constField = testType.GetField("ConstField");
            var field = testType.GetField("Field");
            var readonlyField = testType.GetField("ReadOnlyField");
            var staticField = testType.GetField("StaticField");
            var staticReadonlyField = testType.GetField("StaticReadOnlyField");

            var constFieldGetter = FluentIl.CreateGetter(constField);
            var fieldGetter = FluentIl.CreateGetter(field);
            var readonlyFieldGetter = FluentIl.CreateGetter(readonlyField);
            var staticFieldGetter = FluentIl.CreateGetter(staticField);
            var staticReadonlyFieldGetter = FluentIl.CreateGetter(staticReadonlyField);

            var constFieldSetter = FluentIl.CreateSetter(constField);
            var fieldSetter = FluentIl.CreateSetter(field);
            var readonlyFieldSetter = FluentIl.CreateSetter(readonlyField);
            var staticFieldSetter = FluentIl.CreateSetter(staticField);
            var staticReadonlyFieldSetter = FluentIl.CreateSetter(staticReadonlyField);


            Assert.IsNotNull(constFieldGetter, "Field getter equals null");
            Assert.IsNotNull(fieldGetter, "ReadOnlyField getter equals null");
            Assert.IsNotNull(readonlyFieldGetter, "ReadOnlyField getter equals null");
            Assert.IsNotNull(staticFieldGetter, "StaticField getter equals null");
            Assert.IsNotNull(staticReadonlyFieldGetter, "StaticReadOnlyField getter equals null");

            Assert.IsNull(constFieldSetter, "ConstField must equal null");
            Assert.IsNotNull(fieldSetter, "Field getter equals null");
            Assert.IsNull(readonlyFieldSetter, "ReadOnlyField must equal null");
            Assert.IsNotNull(staticFieldSetter, "StaticField getter equals null");
            Assert.IsNull(staticReadonlyFieldSetter, "StaticReadOnlyField must equal null");

            Assert.AreEqual(constFieldGetter(null), "TestString");
            Assert.AreEqual(fieldGetter(testInstance), "TestString");
            Assert.AreEqual(readonlyFieldGetter(testInstance), "TestString");
            Assert.AreEqual(staticFieldGetter(null), "TestString");
            Assert.AreEqual(staticReadonlyFieldGetter(null), "TestString");

            fieldSetter(testInstance, "OtherTestString");
            staticFieldSetter(null, "OtherTestString");

            Assert.AreEqual(testInstance.Field, "OtherTestString");
            Assert.AreEqual(FluentIlTestClass.StaticField, "OtherTestString");
        }

        [TestMethod]
        public void DynamicGetterSetterPropertyTest() {
            var testType = typeof(FluentIlTestClass);
            var testInstance = new FluentIlTestClass();

            var property = testType.GetProperty("Property");
            var readonlyProperty = testType.GetProperty("ReadOnlyProperty");
            var staticProperty = testType.GetProperty("StaticProperty");
            var staticReadonlyProperty = testType.GetProperty("StaticReadOnlyProperty");

            var propertyGetter = FluentIl.CreateGetter(property);
            var readonlyPropertyGetter = FluentIl.CreateGetter(readonlyProperty);
            var staticPropertyGetter = FluentIl.CreateGetter(staticProperty);
            var staticReadonlyPropertyGetter = FluentIl.CreateGetter(staticReadonlyProperty);

            var propertySetter = FluentIl.CreateSetter(property);
            var readonlyPropertySetter = FluentIl.CreateSetter(readonlyProperty);
            var staticPropertySetter = FluentIl.CreateSetter(staticProperty);
            var staticReadonlyPropertySetter = FluentIl.CreateSetter(staticReadonlyProperty);


            Assert.IsNotNull(propertyGetter, "ReadOnlyProperty getter equals null");
            Assert.IsNotNull(readonlyPropertyGetter, "ReadOnlyProperty getter equals null");
            Assert.IsNotNull(staticPropertyGetter, "StaticProperty getter equals null");
            Assert.IsNotNull(staticReadonlyPropertyGetter, "StaticReadOnlyProperty getter equals null");

            Assert.IsNotNull(propertySetter, "Property getter equals null");
            Assert.IsNull(readonlyPropertySetter, "ReadOnlyProperty must equal null");
            Assert.IsNotNull(staticPropertySetter, "StaticProperty getter equals null");
            Assert.IsNull(staticReadonlyPropertySetter, "StaticReadOnlyProperty must equal null");

            Assert.AreEqual(propertyGetter(testInstance), "TestString");
            Assert.AreEqual(readonlyPropertyGetter(testInstance), "TestString");
            Assert.AreEqual(staticPropertyGetter(null), "TestString");
            Assert.AreEqual(staticReadonlyPropertyGetter(null), "TestString");

            propertySetter(testInstance, "OtherTestString");
            staticPropertySetter(null, "OtherTestString");

            Assert.AreEqual(testInstance.Property, "OtherTestString");
            Assert.AreEqual(FluentIlTestClass.StaticProperty, "OtherTestString");
        }

        public class FluentIlTestClass {
            public const string ConstField = "TestString";
            public static string StaticField = "TestString";
            public static readonly string StaticReadOnlyField = "TestString";
            public readonly string ReadOnlyField = "TestString";
            public string Field = "TestString";
            public string Property { get; set; } = "TestString";
            public string ReadOnlyProperty { get; } = "TestString";
            public static string StaticProperty { get; set; } = "TestString";
            public static string StaticReadOnlyProperty { get; } = "TestString";
        }
    }
}
