// ==================================================
// Copyright 2016(C) , DotLogix
// File:  XmlExtension.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  06.09.2017
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
