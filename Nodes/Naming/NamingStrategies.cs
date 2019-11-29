// ==================================================
// Copyright 2019(C) , DotLogix
// File:  NamingStrategies.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  ..
// LastEdited:  08.06.2019
// ==================================================

using DotLogix.Core.Nodes.Processor;

namespace DotLogix.Core.Nodes {
    /// <summary>
    /// Common naming strategies
    /// </summary>
    public static class NamingStrategies {
        /// <summary>
        /// A camelCase naming strategy
        /// </summary>
        public static INamingStrategy CamelCase => new CamelCaseNamingStrategy();
        /// <summary>
        /// A kebap-case naming strategy
        /// </summary>
        public static INamingStrategy KebapCase => new KebapCaseNamingStrategy();
        /// <summary>
        /// A lowercase naming strategy
        /// </summary>
        public static INamingStrategy LowerCase => new LowerCaseNamingStrategy();
        /// <summary>
        /// A PascalCase naming strategy
        /// </summary>
        public static INamingStrategy PascalCase => new PascalCaseNamingStrategy();
        /// <summary>
        /// A snake_case naming strategy
        /// </summary>
        public static INamingStrategy SnakeCase => new SnakeCaseNamingStrategy();
        /// <summary>
        /// A UPPERCASE naming strategy
        /// </summary>
        public static INamingStrategy UpperCase => new UpperCaseNamingStrategy();
    }
}