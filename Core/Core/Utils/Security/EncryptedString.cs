// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EncryptedString.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
#endregion

namespace DotLogix.Core.Utils.Security {
    /// <summary>
    /// A safe representation of a string stored in undetectable unmanaged memory
    /// </summary>
    [TypeConverter(typeof(EncryptedStringConverter))]
    public class EncryptedString {
        private IntPtr _pointer;

        /// <summary>
        /// Get the clear text
        /// </summary>
        public string ClearText {
            get => ToClearText();
            set => SetClearText(value);
        }

        /// <summary>
        /// The length of the string
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Creates a new <see cref="EncryptedString"/>
        /// </summary>
        public EncryptedString(string clearText) {
            SetClearText(clearText);
        }

        /// <summary>
        /// Creates a new <see cref="EncryptedString"/>
        /// </summary>
        public EncryptedString() { }

        /// <summary>
        /// Checks if the plain text equals
        /// </summary>
        protected bool Equals(EncryptedString other) {
            return ClearText.Equals(other?.ClearText);
        }

        /// <summary>
        /// Checks if the plain text equals
        /// </summary>

        public override bool Equals(object obj) {
            if(ReferenceEquals(null, obj))
                return false;
            if(ReferenceEquals(this, obj))
                return true;
            if(obj.GetType() != GetType())
                return false;
            return Equals((EncryptedString)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode() {
            return ClearText.GetHashCode();
        }

        /// <summary>
        /// Replace the text with another
        /// </summary>
        public void SetClearText(string clearText) {
            if(_pointer != IntPtr.Zero)
                Marshal.FreeHGlobal(_pointer);
            Length = clearText.Length;
            _pointer = Marshal.StringToHGlobalUni(clearText); //Will return IntPtr.Zero if value is null
        }

        /// <summary>
        /// Get the clear text
        /// </summary>

        public string ToClearText() {
            return Marshal.PtrToStringUni(_pointer);
        }

        /// <summary>
        /// Get an obfuscated representation of the string
        /// </summary>
        public string ToObfuscatedString() {
            var plain = ToClearText();
            return Encryption.ObfuscateXor(plain);
        }

        /// <summary>
        /// Get an encrypted representation of the string using a random guid as key.<br></br>
        /// The string can be decoded with this class as well.
        /// </summary>
        public string ToEncryptedString() {
            var plainText = ToClearText();
            if(plainText == null)
                throw new InvalidOperationException("Empty string can not be encrypted");

            var guid = Guid.NewGuid();
            var password = guid.ToByteArray();

            var clearBytes = GetBytes();
            var obfuscatedBytes = Encryption.ObfuscateXor(clearBytes);
            var encryptedBytes = Encryption.EncryptAes(obfuscatedBytes, password);

            const int passwordLength = 16; //password Length 
            var encryptedLength = encryptedBytes.Length;

            var clearData = new byte[passwordLength + encryptedLength];
            Buffer.BlockCopy(password, 0, clearData, 0, passwordLength);
            Buffer.BlockCopy(encryptedBytes, 0, clearData, passwordLength, encryptedLength);

            var obfuscatedData = Encryption.ObfuscateXor(clearData);

            return Convert.ToBase64String(obfuscatedData);
        }

        /// <summary>
        /// Get the utf-8 bytes of the string
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes() {
            return Encoding.UTF8.GetBytes(ToClearText());
        }

        /// <summary>
        /// Get a secure string out of this encrypted string
        /// </summary>
        /// <returns></returns>
        public SecureString ToSecureString() {
            unsafe {
                var plain = ToClearText();
                fixed(char* array = plain)
                    return new SecureString(array, plain.Length);
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="EncryptedString"/> using the plain text
        /// </summary>
        public static EncryptedString FromClearText(string plain) {
            return new(plain);
        }

        /// <summary>
        /// Creates a new instance of <see cref="EncryptedString"/> using the obfuscated representation
        /// </summary>
        public static EncryptedString FromObfuscatedString(string obfuscated) {
            var plain = Encryption.UnobfuscateXor(obfuscated);
            return new EncryptedString(plain);
        }

        /// <summary>
        /// Creates a new instance of <see cref="EncryptedString"/> using the encrypted representation
        /// </summary>
        public static EncryptedString FromEncryptedString(string encrypted) {
            if((encrypted == null) || (encrypted.Length <= 16))
                throw new ArgumentException("String can not be decrypted", nameof(encrypted));
            var obfuscatedData = Convert.FromBase64String(encrypted);
            var clearData = Encryption.UnobfuscateXor(obfuscatedData);
            const int passwordLength = 16;
            var encryptedLength = clearData.Length - passwordLength;

            var password = new byte[passwordLength];
            var encryptedBytes = new byte[encryptedLength];
            Buffer.BlockCopy(clearData, 0, password, 0, passwordLength);
            Buffer.BlockCopy(clearData, passwordLength, encryptedBytes, 0, encryptedLength);

            var obfuscatedBytes = Encryption.DecryptAes(encryptedBytes, password);
            var clearBytes = Encryption.UnobfuscateXor(obfuscatedBytes);
            var plainText = Encoding.UTF8.GetString(clearBytes);
            return new EncryptedString(plainText);
        }
    }
}
