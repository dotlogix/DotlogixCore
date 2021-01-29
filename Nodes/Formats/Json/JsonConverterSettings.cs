// ==================================================
// Copyright 2018(C) , DotLogix
// File:  JsonNodesFormatter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  04.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
#endregion

using DotLogix.Core.Nodes.Converters;
using DotLogix.Core.Nodes.Schema;

namespace DotLogix.Core.Nodes.Formats.Json {
    /// <summary>
    ///     Basic node converter settings for json data
    /// </summary>
    public class JsonConverterSettings : ConverterSettings {
        public new static INodeConverterResolver DefaultResolver => CreateDefaultResolver();

        private static INodeConverterResolver CreateDefaultResolver() {
            var resolver = new NodeConverterResolver();
            resolver.Add(new ObjectNodeConverterFactory());
            resolver.Add(new OptionalNodeConverterFactory());
            resolver.Add(new KeyValuePairNodeConverterFactory());
            resolver.Add(new CollectionNodeConverterFactory());
            resolver.Add(new DictionaryObjectConverterFactory());
            resolver.Add(new JsonValueNodeConverterFactory());
            return resolver;
        }
        
        /// <summary>
        ///     Creates new json settings with ident
        /// </summary>
        public static JsonConverterSettings Idented => new JsonConverterSettings(DefaultResolver) { Ident = true };

        /// <summary>
        ///     Creates new json settings
        /// </summary>
        public new static JsonConverterSettings Default => new JsonConverterSettings(DefaultResolver);

        /// <summary>
        ///     Determines if the json text should be idented
        /// </summary>
        public bool Ident { get; set; }

        /// <summary>
        ///     The amount of spaces to insert for ident (4 by default)
        /// </summary>
        public int IdentSize { get; set; }

        /// <summary>
        ///     The options for json parsing
        /// </summary>
        public JsonReaderOptions ReadOptions { get; set; }

        /// <summary>
        ///     The options for json parsing
        /// </summary>
        public JsonWriterOptions WriteOptions { get; set; }

        public JsonConverterSettings(INodeConverterResolver resolver) : base(resolver) { }
    }
}
