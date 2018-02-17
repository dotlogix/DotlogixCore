// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EncryptedStringConverter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.ComponentModel;
using System.Globalization;
#endregion

namespace DotLogix.Core.Security {
    public class EncryptedStringConverter : TypeConverter {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            return sourceType == typeof(string);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            return destinationType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
            var stringValue = value as string;
            if(stringValue == null)
                throw new ArgumentNullException(nameof(value));
            return EncryptedString.FromEncryptedString(stringValue);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType) {
            var encryptedString = value as EncryptedString;
            if(encryptedString == null)
                throw new ArgumentNullException(nameof(value));
            return encryptedString.ToEncryptedString();
        }

        public override bool IsValid(ITypeDescriptorContext context, object value) {
            var stringValue = value as string;
            return (stringValue != null) && (stringValue.Length > 16);
        }
    }
}
