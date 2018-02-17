// ==================================================
// Copyright 2018(C) , DotLogix
// File:  XmlExtension.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Xml.Linq;
#endregion

namespace DotLogix.Core.Extensions {
    public static class XmlExtension {
        public static string GetElementValue(this XElement xParent, string name) {
            var xElement = xParent.Element(name);
            return xElement?.Value;
        }

        public static string GetAttributeValue(this XElement xParent, string name) {
            var xAttribute = xParent.Attribute(name);
            return xAttribute?.Value;
        }
    }
}
