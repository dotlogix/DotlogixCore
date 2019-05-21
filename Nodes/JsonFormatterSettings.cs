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
    public class JsonFormatterSettings : ConverterSettings{
        public static JsonFormatterSettings Idented => new JsonFormatterSettings{Ident = true};
        public new static JsonFormatterSettings Default => new JsonFormatterSettings();

        public bool Ident { get; set; }
        public int IdentSize { get; set; } = 4;
    }
}
