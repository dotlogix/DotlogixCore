// ==================================================
// Copyright 2019(C) , DotLogix
// File:  NamingStrategies.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  ..
// LastEdited:  08.06.2019
// ==================================================

namespace DotLogix.Core.Utils.Naming {
    /// <summary>
    /// Common naming strategies
    /// </summary>
    public static class NamingStrategies {
        /// <summary>
        /// A camelCase naming strategy
        /// </summary>
        public static INamingStrategy CamelCase { get; } = new CamelCaseNamingStrategy();
        /// <summary>
        /// A kebap-case naming strategy
        /// </summary>
        public static INamingStrategy KebapCase { get; } = new KebapCaseNamingStrategy();
        /// <summary>
        /// A lowercase naming strategy
        /// </summary>
        public static INamingStrategy LowerCase { get; } = new LowerCaseNamingStrategy();
        /// <summary>
        /// A PascalCase naming strategy
        /// </summary>
        public static INamingStrategy PascalCase { get; } = new PascalCaseNamingStrategy();
        /// <summary>
        /// A snake_case naming strategy
        /// </summary>
        public static INamingStrategy SnakeCase { get; } = new SnakeCaseNamingStrategy();
        /// <summary>
        /// A snake_case naming strategy
        /// </summary>
        public static INamingStrategy UpperSnakeCase { get; } = new UpperSnakeCaseNamingStrategy();
        /// <summary>
        /// A UPPERCASE naming strategy
        /// </summary>
        public static INamingStrategy UpperCase { get; } = new UpperCaseNamingStrategy();
    }
}