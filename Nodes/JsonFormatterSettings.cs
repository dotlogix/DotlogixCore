// ==================================================
// Copyright 2018(C) , DotLogix
// File:  JsonNodesFormatter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  04.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
#endregion

namespace DotLogix.Core.Nodes {
    /// <summary>
    ///     Basic node converter settings for json data
    /// </summary>
    public class JsonFormatterSettings : ConverterSettings {

        /// <summary>
        ///     Creates new json settings with ident
        /// </summary>
        public static JsonFormatterSettings Idented => new JsonFormatterSettings {Ident = true};

        /// <summary>
        ///     Creates new json settings
        /// </summary>
        public new static JsonFormatterSettings Default => new JsonFormatterSettings();

        /// <summary>
        ///     Determines if the json text should be idented
        /// </summary>
        public bool Ident { get; set; }

        /// <summary>
        ///     The amount of spaces to insert for ident (4 by default)
        /// </summary>
        public int IdentSize { get; set; }

        public override void Apply(IConverterSettings settings) {
            base.Apply(settings);
            if (settings is JsonFormatterSettings jsonSettings) {
                Ident = jsonSettings.Ident;
                IdentSize = jsonSettings.IdentSize;
            }
        }
    }
}
