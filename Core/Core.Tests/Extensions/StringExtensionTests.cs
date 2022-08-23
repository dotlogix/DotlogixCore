using System.Collections.Generic;
using DotLogix.Core.Extensions;
using NUnit.Framework;

namespace Core.Tests.Extensions {
    [TestFixture]
    public class StringExtensionTests {
        [Test]
        public void GetTrigramDistance_ChateauBlanc() {
            const string testString = "chateau blanc";
            var expected = new HashSet<string> {"cha", "hat", "ate", "tea", "eau", "au ", "u b"," bl","bla", "lan", "anc"};
            var actual = testString.ExtractNGrams();
            
            Assert.That(actual, Is.EquivalentTo(expected));
        }
        
        [Test]
        public void GetTrigramDistance_TheQuickRedFox() {
            const string testString = "the quick red fox";
            var expected = new HashSet<string> {"the", "he ", "e q", " qu", "qui", "uic", "ick", "ck ", "k r", " re", "red", "ed ", "d f", " fo", "fox"};
            var actual = testString.ExtractNGrams();
            
            Assert.That(actual, Is.EquivalentTo(expected));
        }
    }
}