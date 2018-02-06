//using DotLogix.Core.Reflection;
//using DotLogix.Core.Reflection.Dynamics;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Test {
//    [TestClass]
//    public class FluentIlDynamicsTest
//    {
//        public class FluentIlTestClass {
//            public const string ConstField = "TestString";
//            public string Field = "TestString";
//            public readonly string ReadOnlyField = "TestString";
//            public static string StaticField = "TestString";
//            public static readonly string StaticReadOnlyField = "TestString";
//            public string Property { get; set; } = "TestString";
//            public string ReadOnlyProperty { get; } = "TestString";
//            public static string StaticProperty { get; set; } = "TestString";
//            public static string StaticReadOnlyProperty { get; } = "TestString";
//        }

//        [TestMethod]
//        public void DynamicGetterSetterFieldTest()
//        {
//            var testType = typeof(FluentIlTestClass);
//            var testInstance = new FluentIlTestClass();

//            var dynamicType = testType.CreateDynamicType(MapModes.Fields);

//            var constField = dynamicType.GetMember("ConstField");
//            var field = dynamicType.GetMember("Field");
//            var readonlyField = dynamicType.GetMember("ReadOnlyField");
//            var staticField = dynamicType.GetMember("StaticField");
//            var staticReadonlyField = dynamicType.GetMember("StaticReadOnlyField");

//            Assert.AreEqual(MemberAccessModes.Read | MemberAccessModes.Static | MemberAccessModes.Const, constField.AccessModes);
//            Assert.AreEqual(MemberAccessModes.ReadWrite, field.AccessModes);
//            Assert.AreEqual(MemberAccessModes.Read, readonlyField.AccessModes);
//            Assert.AreEqual(MemberAccessModes.ReadWrite | MemberAccessModes.Static, staticField.AccessModes);
//            Assert.AreEqual(MemberAccessModes.Read | MemberAccessModes.Static, staticReadonlyField.AccessModes);


//            var constFieldGetter = constField.GetterFunc;
//            var fieldGetter = field.GetterFunc;
//            var readonlyFieldGetter = readonlyField.GetterFunc;
//            var staticFieldGetter = staticField.GetterFunc;
//            var staticReadonlyFieldGetter = staticReadonlyField.GetterFunc;

//            var constFieldSetter = constField.SetterFunc;
//            var fieldSetter = field.SetterFunc;
//            var readonlyFieldSetter = readonlyField.SetterFunc;
//            var staticFieldSetter = staticField.SetterFunc;
//            var staticReadonlyFieldSetter = staticReadonlyField.SetterFunc;


//            Assert.IsNotNull(constFieldGetter, "Field getter equals null");
//            Assert.IsNotNull(fieldGetter, "ReadOnlyField getter equals null");
//            Assert.IsNotNull(readonlyFieldGetter, "ReadOnlyField getter equals null");
//            Assert.IsNotNull(staticFieldGetter, "StaticField getter equals null");
//            Assert.IsNotNull(staticReadonlyFieldGetter, "StaticReadOnlyField getter equals null");

//            Assert.IsNull(constFieldSetter, "ConstField must equal null");
//            Assert.IsNotNull(fieldSetter, "Field getter equals null");
//            Assert.IsNull(readonlyFieldSetter, "ReadOnlyField must equal null");
//            Assert.IsNotNull(staticFieldSetter, "StaticField getter equals null");
//            Assert.IsNull(staticReadonlyFieldSetter, "StaticReadOnlyField must equal null");

//            Assert.AreEqual(constFieldGetter(null), "TestString");
//            Assert.AreEqual(fieldGetter(testInstance), "TestString");
//            Assert.AreEqual(readonlyFieldGetter(testInstance), "TestString");
//            Assert.AreEqual(staticFieldGetter(null), "TestString");
//            Assert.AreEqual(staticReadonlyFieldGetter(null), "TestString");

//            fieldSetter(testInstance, "OtherTestString");
//            staticFieldSetter(null, "OtherTestString");

//            Assert.AreEqual(testInstance.Field, "OtherTestString");
//            Assert.AreEqual(FluentIlTestClass.StaticField, "OtherTestString");
//        }

//        [TestMethod]
//        public void DynamicGetterSetterPropertyTest()
//        {
//            var testType = typeof(FluentIlTestClass);
//            var testInstance = new FluentIlTestClass();

//            var dynamicType = testType.CreateDynamicType();

//            var property = dynamicType.GetMember("Property");
//            var readonlyProperty = dynamicType.GetMember("ReadOnlyProperty");
//            var staticProperty = dynamicType.GetMember("StaticProperty");
//            var staticReadonlyProperty = dynamicType.GetMember("StaticReadOnlyProperty");

//            Assert.AreEqual(MemberAccessModes.ReadWrite, property.AccessModes);
//            Assert.AreEqual(MemberAccessModes.Read, readonlyProperty.AccessModes);
//            Assert.AreEqual(MemberAccessModes.ReadWrite | MemberAccessModes.Static, staticProperty.AccessModes);
//            Assert.AreEqual(MemberAccessModes.Read | MemberAccessModes.Static, staticReadonlyProperty.AccessModes);


//            var propertyGetter = property.GetterFunc;
//            var readonlyPropertyGetter = readonlyProperty.GetterFunc;
//            var staticPropertyGetter = staticProperty.GetterFunc;
//            var staticReadonlyPropertyGetter = staticReadonlyProperty.GetterFunc;

//            var propertySetter = property.SetterFunc;
//            var readonlyPropertySetter = readonlyProperty.SetterFunc;
//            var staticPropertySetter = staticProperty.SetterFunc;
//            var staticReadonlyPropertySetter = staticReadonlyProperty.SetterFunc;


//            Assert.IsNotNull(propertyGetter, "ReadOnlyProperty getter equals null");
//            Assert.IsNotNull(readonlyPropertyGetter, "ReadOnlyProperty getter equals null");
//            Assert.IsNotNull(staticPropertyGetter, "StaticProperty getter equals null");
//            Assert.IsNotNull(staticReadonlyPropertyGetter, "StaticReadOnlyProperty getter equals null");

//            Assert.IsNotNull(propertySetter, "Property getter equals null");
//            Assert.IsNull(readonlyPropertySetter, "ReadOnlyProperty must equal null");
//            Assert.IsNotNull(staticPropertySetter, "StaticProperty getter equals null");
//            Assert.IsNull(staticReadonlyPropertySetter, "StaticReadOnlyProperty must equal null");

//            Assert.AreEqual(propertyGetter(testInstance), "TestString");
//            Assert.AreEqual(readonlyPropertyGetter(testInstance), "TestString");
//            Assert.AreEqual(staticPropertyGetter(null), "TestString");
//            Assert.AreEqual(staticReadonlyPropertyGetter(null), "TestString");

//            propertySetter(testInstance, "OtherTestString");
//            staticPropertySetter(null, "OtherTestString");

//            Assert.AreEqual(testInstance.Property, "OtherTestString");
//            Assert.AreEqual(FluentIlTestClass.StaticProperty, "OtherTestString");
//        }
//    }
//}