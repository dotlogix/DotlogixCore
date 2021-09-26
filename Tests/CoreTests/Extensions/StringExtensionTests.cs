using System.Collections.Generic;
using DotLogix.Core.Extensions;
using NUnit.Framework;

namespace CoreTests.Extensions {
    [TestFixture]
    public class StringExtensionTests {
        [Test]
        public void GetTrigramDistance() {
            var testString = "chateau  blanc";
            var expected = new HashSet<string> {"  c", " ch", "cha", "hat", "ate", "tea", "eau", "au ", "u  ", "  b"," bl","bla", "lan", "anc", "nc "};
            var actual = testString.ExtractNGrams();
            
            Assert.That(actual, Is.EquivalentTo(expected));
//            
//            
//            
//            
//            
//            var trigramStr2 = str2.ExtractNGrams();
//            
//            
//            
//            var hammingDistance = str1.GetHammingDistance(str2 + " ");
//            var levenshteinDistance = str1.GetLevenshteinDistance(str2);
//            var damerauLevenshteinDistance = str1.GetDamerauLevenshteinDistance(str2);
//            
//            Assert.True(true);
            
            
            
        }
    }
}
