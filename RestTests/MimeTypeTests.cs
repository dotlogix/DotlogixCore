using System;
using System.Text.RegularExpressions;
using DotLogix.Core.Nodes;
using DotLogix.Core.Nodes.Formats.Json;
using DotLogix.Core.Rest.Http;
using DotLogix.Core.Rest.Http.Headers;
using NUnit.Framework;

namespace RestTests {
    [TestFixture]
    public class MimeTypeTests {
        
        [SetUp]
        public void Setup() {

        }

        [Test]
        public void MimeType_Invalid() {
            var mimeTypeStr = "This is not a valid mime type; test;=1";
            Assert.That(() => MimeType.Parse(mimeTypeStr), Throws.TypeOf<FormatException>());
        }

        [Test]
        public void MimeType_Regex() {
            var jsonValue = MimeTypes.Application.Json.Value;
            var value = jsonValue + ";q=0.3;v;v=test;a    ;b;    c     =    abcde;";

            var tokenRegex = "(?:[!#$%&'*+-.^_`|~'\\d\\w]+)";
            var attributeRegex = "(<attrName>"+tokenRegex+ ")(<attrValue>" + tokenRegex+"\\s*=\\s*"+ tokenRegex + "|(?:\"[^\"]+\"))";
            var headerRegex = "^(?<code>"+tokenRegex+")(?:\\s*;\\s*"+ attributeRegex + ")+;";
            var partMatch = Regex.Match(value, headerRegex);


        }

        [Test]
        public void MimeType_SimpleParse() {
            var mimeTypeStr = MimeTypes.Application.Json.Value;
            var mimeType = MimeType.Parse(mimeTypeStr);
            Assert.That(mimeType.HasAttributes, Is.False);
            Assert.That(mimeType.Group, Is.EqualTo("application"));
            Assert.That(mimeType.SubType, Is.EqualTo("json"));
            Assert.That(mimeType.Quality, Is.EqualTo(1));

            Assert.That(mimeType, Is.EqualTo(MimeTypes.Application.Json));
        }


        [Test]
        public void MimeType_AttributeParsing() {
            var jsonValue = MimeTypes.Application.Json.Value;
            var mimeTypeJsonStr = jsonValue + ";q=0.3;v;v=test;a    ;b;    c     =    abcde;";
            var mimeType = MimeType.Parse(mimeTypeJsonStr);

            Assert.That(mimeType, Is.EqualTo(MimeTypes.Application.Json));
            Assert.That(mimeType.HasAttributes, Is.True);
            Assert.That(mimeType.Group, Is.EqualTo("application"));
            Assert.That(mimeType.SubType, Is.EqualTo("json"));
            Assert.That(mimeType.Quality, Is.EqualTo(0.3d));

            Assert.That(mimeType.Attributes, Is.Not.Empty);

            string GetExceptionMessage() {
                return JsonUtils.ToJson(mimeType.Attributes, JsonConverterSettings.Idented);
            }

            Assert.That(mimeType.Attributes.Count, Is.EqualTo(5), GetExceptionMessage);


            Assert.That(mimeType.Attributes.TryGetValue("q", out var qValue), Is.True);
            Assert.That(qValue.IsDefined, Is.True);
            Assert.That(qValue.Value, Is.EqualTo("0.3"));

            Assert.That(mimeType.Attributes.TryGetValue("v", out var vValue), Is.True);
            Assert.That(vValue.IsDefined, Is.True);
            Assert.That(vValue.Value, Is.EqualTo("test"));

            Assert.That(mimeType.Attributes.TryGetValue("a", out var aValue), Is.True);
            Assert.That(aValue.IsDefined, Is.False);
            Assert.That(mimeType.Attributes.TryGetValue("b", out var bValue), Is.True);
            Assert.That(bValue.IsDefined, Is.False);
            Assert.That(mimeType.Attributes.TryGetValue("c", out var cValue), Is.True);
            Assert.That(cValue.IsDefined, Is.True);
            Assert.That(cValue.Value, Is.EqualTo("abcde"));

            Assert.That(mimeType.Attributes.TryGetValue("d", out _), Is.False);
        }

        [Test]
        public void MimeType_AttributeSerializing() {
            var jsonValue = MimeTypes.Application.Json;
            jsonValue.Attributes.Add("q", "0.3");
            jsonValue.Attributes.Add("v", "test");
            jsonValue.Attributes.Add("a", default);
            jsonValue.Attributes.Add("b", default);
            jsonValue.Attributes.Add("c", "abcde");

            var expectedString = jsonValue.Value + ";q=0.3;v=test;a;b;c=abcde;";
            var actual = jsonValue.ToString();

            Assert.That(actual, Does.StartWith(jsonValue.Value + ";"));
            Assert.That(actual, Does.Not.EndWith(";"));

            // else the last property would fail
            actual += ";";
            Assert.That(actual, Contains.Substring(";q=0.3;"));
            Assert.That(actual, Contains.Substring(";v=test;"));
            Assert.That(actual, Contains.Substring(";a;"));
            Assert.That(actual, Contains.Substring(";b;"));
            Assert.That(actual, Contains.Substring(";c=abcde;"));

            string GetExceptionMessage() {
                return "Expected (Any Order): " + expectedString + "\n" + "Actual (Any Order):" + actual;
            }

            Assert.That(actual.Length, Is.EqualTo(expectedString.Length), GetExceptionMessage);
        }

        [Test]
        public void MimeType_Properties_ReturnsCorrectValue() {
            var mimeType = MimeTypes.Application.Json;

            Assert.That(mimeType.Group, Is.EqualTo("application"));
            Assert.That(mimeType.SubType, Is.EqualTo("json"));


            mimeType = MimeTypes.Text.Csv;
            Assert.That(mimeType.Group, Is.EqualTo("text"));
            Assert.That(mimeType.SubType, Is.EqualTo("comma-separated-values"));
        }
    }
}
