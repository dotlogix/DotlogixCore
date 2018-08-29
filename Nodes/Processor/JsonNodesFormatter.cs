// ==================================================
// Copyright 2018(C) , DotLogix
// File:  JsonNodesFormatter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  04.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
#endregion

namespace DotLogix.Core.Nodes.Processor {
    public class JsonNodesFormatter {
        public static JsonNodesFormatter Idented => new JsonNodesFormatter{Ident = true};
        public static JsonNodesFormatter Default => new JsonNodesFormatter();

        public bool Ident { get; set; }
        public int IdentSize { get; set; }

        public JsonNodesFormatter() {
            IdentSize = 4;
        }

        public JsonNodesFormatter(JsonNodesFormatter basedOn) {
            Ident = basedOn.Ident;
            IdentSize = basedOn.IdentSize;
        }
    }
}
