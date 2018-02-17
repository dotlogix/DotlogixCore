// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EncryptionTest.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Text;
using DotLogix.Core.Extensions;
using DotLogix.Core.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endregion

namespace Test {
    [TestClass]
    public class EncryptionTest {
        [TestMethod]
        public void TestStringEncryption() {
            const string originalString = "bubedibabedi";

            var encryptedString = Encryption.EncryptAes(originalString, "password");
            var decryptedString = Encryption.DecryptAes(encryptedString, "password");

            Assert.AreEqual(originalString, decryptedString);
        }

        [TestMethod]
        public void TestByteArrayEncryption() {
            const string originalString = "bubedibabedi";
            var originalBytes = Encoding.UTF8.GetBytes(originalString);
            var passwordBytes = Encoding.UTF8.GetBytes("password");

            var encryptedBytes = Encryption.EncryptAes(originalBytes, passwordBytes);
            var decryptedBytes = Encryption.DecryptAes(encryptedBytes, passwordBytes);

            var decryptedString = Encoding.UTF8.GetString(decryptedBytes);
            Assert.AreEqual(originalString, decryptedString);
        }

        [TestMethod]
        public void TestStringObfuscation() {
            const string originalString = "bubedibabedi";

            var encryptedString = Encryption.ObfuscateXor(originalString);
            var decryptedString = Encryption.UnobfuscateXor(encryptedString);

            Assert.AreEqual(originalString, decryptedString);
        }

        [TestMethod]
        public void TestByteArrayObfuscation() {
            const string originalString = "bubedibabedi";
            var originalBytes = Encoding.UTF8.GetBytes(originalString);

            var encryptedBytes = Encryption.ObfuscateXor(originalBytes);
            var decryptedBytes = Encryption.UnobfuscateXor(encryptedBytes);

            var decryptedString = Encoding.UTF8.GetString(decryptedBytes);
            Assert.AreEqual(originalString, decryptedString);
        }

        [TestMethod]
        public void TestEncryptedStringTypeConverter() {
            const string originalString = "bubedibabedi";
            var encryptedString = new EncryptedString(originalString);
            var encryptedStringValue = encryptedString.ConvertTo<string>();

            Assert.AreEqual(EncryptedString.FromEncryptedString(encryptedStringValue), encryptedString);

            var encryptedStringConverted = encryptedStringValue.ConvertTo<EncryptedString>();
            Assert.AreEqual(encryptedStringConverted, encryptedString);
        }
    }
}
