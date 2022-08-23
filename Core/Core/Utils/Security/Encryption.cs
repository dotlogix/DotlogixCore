// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Encryption.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
#endregion

namespace DotLogix.Core.Utils.Security; 

/// <summary>
///     encrypt and decrypt strings
/// </summary>
public static class Encryption {
    private static readonly byte[] Salt = {
        0x49,
        0x76,
        0x61,
        0x6e,
        0x20,
        0x4d,
        0x65,
        0x64,
        0x76,
        0x65,
        0x64,
        0x65,
        0x76
    };

    /// <summary>
    ///     Obfuscates an array of bytes with an XOr-Algorithm
    /// </summary>
    /// <param name="clearBytes">The clear bytes</param>
    /// <returns>A new array of obfuscated bytes</returns>
    public static byte[] ObfuscateXor(byte[] clearBytes) {
        var dataLength = clearBytes.Length;
        var lastIndex = dataLength - 1;

        var obfuscatedBytes = new byte[dataLength];
        for(var i = 0; i < lastIndex; i++)
            obfuscatedBytes[i] = (byte)(clearBytes[i] ^ clearBytes[i + 1]);
        obfuscatedBytes[lastIndex] = (byte)(clearBytes[lastIndex] ^ obfuscatedBytes[0]);
        return obfuscatedBytes;
    }

    /// <summary>
    ///     Unobfuscates an array of obfuscated bytes with an XOr-Algorithm
    /// </summary>
    /// <param name="obfuscatedBytes">The obfuscated bytes</param>
    /// <returns>A new array of clear bytes</returns>
    public static byte[] UnobfuscateXor(byte[] obfuscatedBytes) {
        var dataLength = obfuscatedBytes.Length;
        var lastIndex = dataLength - 1;

        var clearBytes = new byte[dataLength];
        clearBytes[lastIndex] = (byte)(obfuscatedBytes[lastIndex] ^ obfuscatedBytes[0]);
        for(var i = lastIndex - 1; i >= 0; i--)
            clearBytes[i] ^= (byte)(obfuscatedBytes[i] ^ clearBytes[i + 1]);
        return clearBytes;
    }

    /// <summary>
    ///     Obfuscates a clear string with an XOr-Algorithm
    /// </summary>
    /// <param name="clearText">The clear text</param>
    /// <returns>A new obfuscated string in base64 format</returns>
    public static string ObfuscateXor(string clearText) {
        var plainBytes = Encoding.UTF8.GetBytes(clearText);
        var obfuscatedBytes = ObfuscateXor(plainBytes);
        return Convert.ToBase64String(obfuscatedBytes);
    }

    /// <summary>
    ///     Unbfuscates a base64 formatted obfuscated string with an XOr-Algorithm
    /// </summary>
    /// <param name="obfuscatedText">The obfuscated text</param>
    /// <returns>A new clear string</returns>
    public static string UnobfuscateXor(string obfuscatedText) {
        var obfuscatedBytes = Convert.FromBase64String(obfuscatedText);
        var plainBytes = UnobfuscateXor(obfuscatedBytes);
        return Encoding.UTF8.GetString(plainBytes);
    }

    /// <summary>
    ///     Encrypts the data.
    /// </summary>
    /// <param name="clearBytes">The clear bytes</param>
    /// <param name="password">The password bytes</param>
    /// <param name="iterations">The iterations of the algorithm</param>
    /// <returns></returns>
    public static byte[] EncryptAes(byte[] clearBytes, byte[] password, int iterations = 1) {
        var pdb = new Rfc2898DeriveBytes(password, Salt, iterations);
        var encryptedData = EncryptAes(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));
        return encryptedData;
    }

    /// <summary>
    ///     Decrypts the data.
    /// </summary>
    /// <param name="encryptedBytes">The clear bytes</param>
    /// <param name="password">The password bytes</param>
    /// <param name="iterations">The iterations of the algorithm</param>
    /// <returns></returns>
    public static byte[] DecryptAes(byte[] encryptedBytes, EncryptedString password, int iterations = 1) {
        var pdb = new Rfc2898DeriveBytes(password.GetBytes(), Salt, iterations);
        var clearBytes = DecryptAes(encryptedBytes, pdb.GetBytes(32), pdb.GetBytes(16));
        return clearBytes;
    }

    /// <summary>
    ///     Encrypts the data.
    /// </summary>
    /// <param name="clearBytes">The clear bytes</param>
    /// <param name="password">The password bytes</param>
    /// <param name="iterations">The iterations of the algorithm</param>
    /// <returns></returns>
    public static byte[] EncryptAes(byte[] clearBytes, EncryptedString password, int iterations = 1) {
        var pdb = new Rfc2898DeriveBytes(password.GetBytes(), Salt, iterations);
        var encryptedData = EncryptAes(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));
        return encryptedData;
    }

    /// <summary>
    ///     Decrypts the data.
    /// </summary>
    /// <param name="encryptedBytes">The clear bytes</param>
    /// <param name="password">The password bytes</param>
    /// <param name="iterations">The iterations of the algorithm</param>
    /// <returns></returns>
    public static byte[] DecryptAes(byte[] encryptedBytes, byte[] password, int iterations = 1) {
        var pdb = new Rfc2898DeriveBytes(password, Salt, iterations);
        var clearBytes = DecryptAes(encryptedBytes, pdb.GetBytes(32), pdb.GetBytes(16));
        return clearBytes;
    }


    /// <summary>
    ///     Encrypts the string.
    /// </summary>
    /// <param name="clearText">The clear text.</param>
    /// <param name="password">The password.</param>
    /// <param name="iterations">The iterations of the algorithm</param>
    /// <returns></returns>
    public static string EncryptAes(string clearText, EncryptedString password, int iterations = 1) {
        var clearBytes = Encoding.Unicode.GetBytes(clearText);
        var clearPassword = password.GetBytes();
        var encryptedBytes = EncryptAes(clearBytes, clearPassword, iterations);
        return Convert.ToBase64String(encryptedBytes);
    }

    /// <summary>
    ///     Decrypts the string.
    /// </summary>
    /// <param name="encryptedText">The encrypted text.</param>
    /// <param name="password">The password.</param>
    /// <param name="iterations">The iterations of the algorithm</param>
    /// <returns></returns>
    public static string DecryptAes(string encryptedText, EncryptedString password, int iterations = 1) {
        var encryptedBytes = Convert.FromBase64String(encryptedText);
        var clearPassword = password.GetBytes();
        var clearBytes = DecryptAes(encryptedBytes, clearPassword, iterations);
        return Encoding.Unicode.GetString(clearBytes);
    }

    /// <summary>
    ///     Encrypts the string.
    /// </summary>
    /// <param name="clearText">The clear text.</param>
    /// <param name="password">The password.</param>
    /// <param name="iterations">The iterations of the algorithm</param>
    /// <returns></returns>
    public static string EncryptAes(string clearText, string password, int iterations = 1) {
        var clearBytes = Encoding.Unicode.GetBytes(clearText);
        var clearPassword = Encoding.Unicode.GetBytes(password);
        var encryptedBytes = EncryptAes(clearBytes, clearPassword, iterations);
        return Convert.ToBase64String(encryptedBytes);
    }

    /// <summary>
    ///     Decrypts the string.
    /// </summary>
    /// <param name="encryptedText">The encrypted text.</param>
    /// <param name="password">The password.</param>
    /// <param name="iterations">The iterations of the algorithm</param>
    /// <returns></returns>
    public static string DecryptAes(string encryptedText, string password, int iterations = 1) {
        var encryptedBytes = Convert.FromBase64String(encryptedText);
        var clearPassword = Encoding.Unicode.GetBytes(password);
        var clearBytes = DecryptAes(encryptedBytes, clearPassword, iterations);
        return Encoding.Unicode.GetString(clearBytes);
    }

    /// <summary>
    ///     Encrypts the string.
    /// </summary>
    /// <param name="clearBytes">The clear text.</param>
    /// <param name="key">The key.</param>
    /// <param name="iv">The IV.</param>
    /// <returns></returns>
    private static byte[] EncryptAes(byte[] clearBytes, byte[] key, byte[] iv) {
        var ms = new MemoryStream();
        var alg = Aes.Create();
        alg.Key = key;
        alg.IV = iv;
        var cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);
        cs.Write(clearBytes, 0, clearBytes.Length);
        cs.Close();
        var encryptedData = ms.ToArray();
        return encryptedData;
    }

    /// <summary>
    ///     Decrypts the string.
    /// </summary>
    /// <param name="encryptedBytes">The cipher data.</param>
    /// <param name="key">The key.</param>
    /// <param name="iv">The IV.</param>
    /// <returns></returns>
    private static byte[] DecryptAes(byte[] encryptedBytes, byte[] key, byte[] iv) {
        var ms = new MemoryStream();
        var alg = Aes.Create();
        alg.Key = key;
        alg.IV = iv;
        var cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
        cs.Write(encryptedBytes, 0, encryptedBytes.Length);
        cs.Close();
        var decryptedData = ms.ToArray();
        return decryptedData;
    }
}