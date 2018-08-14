// ==================================================
// Copyright 2018(C) , DotLogix
// File:  XmlExtension.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Xml.Linq;
#endregion

namespace DotLogix.Core.Extensions {
    public static class XmlExtension {
        /// <summary>
        ///     Get the value of a descendent of an xml element
        /// </summary>
        /// <param name="xParent">The parent element</param>
        /// <param name="name">The name of the descendant</param>
        /// <returns></returns>
        public static string GetElementValue(this XElement xParent, string name) {
            var xElement = xParent.Element(name);
            return xElement?.Value;
        }

        /// <summary>
        ///     Get the value of an attribute of an xml element
        /// </summary>
        /// <param name="xParent">The parent element</param>
        /// <param name="name">The name of the attribute</param>
        /// <returns></returns>
        public static string GetAttributeValue(this XElement xParent, string name) {
            var xAttribute = xParent.Attribute(name);
            return xAttribute?.Value;
        }
    }
}
