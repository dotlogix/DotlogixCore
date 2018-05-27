// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EncryptedString.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
#endregion

namespace DotLogix.Core.Security {
    [TypeConverter(typeof(EncryptedStringConverter))]
    public class EncryptedString {
        private IntPtr _pointer;

        public string ClearText {
            get => ToClearText();
            set => SetClearText(value);
        }

        public int Length { get; private set; }

        public EncryptedString(string clearText) {
            SetClearText(clearText);
        }

        public EncryptedString() { }

        protected bool Equals(EncryptedString other) {
            return ClearText.Equals(other?.ClearText);
        }

        public override bool Equals(object obj) {
            if(ReferenceEquals(null, obj))
                return false;
            if(ReferenceEquals(this, obj))
                return true;
            if(obj.GetType() != GetType())
                return false;
            return Equals((EncryptedString)obj);
        }

        public override int GetHashCode() {
            return ClearText.GetHashCode();
        }

        public void SetClearText(string clearText) {
            if(_pointer != IntPtr.Zero)
                Marshal.FreeHGlobal(_pointer);
            Length = clearText.Length;
            _pointer = Marshal.StringToHGlobalUni(clearText); //Will return IntPtr.Zero if value is null
        }


        public string ToClearText() {
            return Marshal.PtrToStringUni(_pointer);
        }

        public string ToObfuscatedString() {
            var plain = ToClearText();
            return Encryption.ObfuscateXor(plain);
        }

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

        public byte[] GetBytes() {
            return Encoding.UTF8.GetBytes(ToClearText());
        }

        public SecureString ToSecureString() {
            unsafe {
                var plain = ToClearText();
                fixed(char* array = plain)
                    return new SecureString(array, plain.Length);
            }
        }

        public static EncryptedString FromClearText(string plain) {
            return new EncryptedString(plain);
        }

        public static EncryptedString FromObfuscatedString(string obfuscated) {
            var plain = Encryption.UnobfuscateXor(obfuscated);
            return new EncryptedString(plain);
        }

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
