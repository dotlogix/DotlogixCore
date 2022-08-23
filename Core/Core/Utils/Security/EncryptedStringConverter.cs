// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EncryptedStringConverter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.ComponentModel;
using System.Globalization;
#endregion

namespace DotLogix.Core.Utils.Security; 

/// <summary>
/// A type converter to convert encrypted strings
/// </summary>
public class EncryptedStringConverter : TypeConverter {
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
        return sourceType == typeof(string);
    }

    /// <inheritdoc />
    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
        return destinationType == typeof(string);
    }

    /// <inheritdoc />
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
        if(value is not string stringValue)
            throw new ArgumentNullException(nameof(value));
        return EncryptedString.FromEncryptedString(stringValue);
    }

    /// <inheritdoc />
    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
        if(value is not EncryptedString encryptedString)
            throw new ArgumentNullException(nameof(value));
        return encryptedString.ToEncryptedString();
    }

    /// <inheritdoc />
    public override bool IsValid(ITypeDescriptorContext context, object value) {
        return value is string { Length: > 16 };
    }
}