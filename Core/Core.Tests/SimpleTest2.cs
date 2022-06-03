using DotLogix.Core;
using DotLogix.Core.Reflection.Dynamics;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoreTests {
    [TestFixture]
    public class SimpleTest2 {
        [Test]
        public void TestMethod() {
            var ctor = typeof(Optional<>).MakeGenericType(typeof(TestEnum)).CreateDynamicType().GetConstructor(typeof(TestEnum));

            var deserialize = JsonConvert.DeserializeObject("0", typeof(TestEnum));
            
            var value = ctor.Invoke(deserialize);
            Assert.That(((Optional<TestEnum>)value).Value, Is.EqualTo(TestEnum.Test));
        }
    }
}
