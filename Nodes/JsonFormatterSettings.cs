// ==================================================
// Copyright 2018(C) , DotLogix
// File:  JsonNodesFormatter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  04.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
#endregion

using DotLogix.Core.Utils;

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
        public bool Ident {
            get => GetWithMemberName(false);
            set => SetWithMemberName(value);
        }

        /// <summary>
        ///     The amount of spaces to insert for ident (4 by default)
        /// </summary>
        public int IdentSize {
            get => GetWithMemberName(4);
            set => SetWithMemberName(value);
        }

        /// <summary>
        /// Creates a new instance of <see cref="ConverterSettings.Settings"/>
        /// </summary>
        public JsonFormatterSettings() {
            Resolver = Nodes.DefaultJsonResolver;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ConverterSettings.Settings"/>
        /// </summary>
        protected JsonFormatterSettings(ISettings settings) : base(settings) {
            Resolver = Nodes.DefaultJsonResolver;
        }
    }
}
