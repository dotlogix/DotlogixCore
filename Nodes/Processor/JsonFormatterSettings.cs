// ==================================================
// Copyright 2018(C) , DotLogix
// File:  JsonNodesFormatter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  04.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
#endregion

using System;
using System.Globalization;
using System.Text;
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Nodes.Processor {
    public class ConverterSettings {
        public INamingStrategy NamingStrategy { get; set; } = new CamelCaseNamingStrategy();

        public string TimeFormat { get; set; } = "u";
        public string NumberFormat { get; set; } = "G";
        public string GuidFormat { get; set; } = "D";
        public string EnumFormat { get; set; } = "D";

        public IFormatProvider FormatProvider { get; set; } = CultureInfo.InvariantCulture;
    }

    public interface INamingStrategy {
        string TransformName(string name);
        void AppendName(string name, StringBuilder builder);
    }

    public abstract class NamingStrategyBase : INamingStrategy {
        private readonly StringBuilder _builder = new StringBuilder(50);

        public string TransformName(string name) {
            if(TransformIfRequired(name, _builder)) {
                name = _builder.ToString();
                _builder.Clear();
            }
            return name;
        }
        public void AppendName(string name, StringBuilder builder) {
            if(TransformIfRequired(name, builder) == false)
                builder.Append(name);
        }

        protected abstract bool TransformIfRequired(string name, StringBuilder builder);
    }



    public class CamelCaseNamingStrategy : NamingStrategyBase {
        protected override bool TransformIfRequired(string name, StringBuilder builder) {
            var first = name[0];
            var firstLc = char.ToLowerInvariant(first);
            if(first != firstLc) {
                builder.Append(firstLc);
                builder.Append(name, 1, name.Length - 1);
                return true;
            }
            return false;
        }
    }

    public class PascalCaseNamingStrategy : NamingStrategyBase {
        protected override bool TransformIfRequired(string name, StringBuilder builder) {
            var first = name[0];
            var firstUc = char.ToUpperInvariant(first);
            if(first != firstUc) {
                builder.Append(firstUc);
                builder.Append(name, 1, name.Length - 1);
                return true;
            }
            return false;
        }
    }
    public class SnakeCaseNamingStrategy : NamingStrategyBase {
        protected override bool TransformIfRequired(string name, StringBuilder builder) {
            for(var i = 0; i < name.Length; i++) {
                var chr = name[i];
                var lc = char.ToLowerInvariant(chr);

                if(i > 0 && chr != lc)
                    builder.Append('_');
                builder.Append(lc);
            }
            return true;
        }
    }
    public class KebapCaseNamingStrategy : NamingStrategyBase {
        protected override bool TransformIfRequired(string name, StringBuilder builder) {
            for(var i = 0; i < name.Length; i++) {
                var chr = name[i];
                var lc = char.ToLowerInvariant(chr);

                if(i > 0 && chr != lc)
                    builder.Append('-');
                builder.Append(lc);
            }

            return true;
        }
    }
    public class LowerCaseNamingStrategy : NamingStrategyBase {
        protected override bool TransformIfRequired(string name, StringBuilder builder) {
            foreach(var chr in name)
                builder.Append(char.ToLowerInvariant(chr));
            return true;
        }
    }
    public class UpperCaseNamingStrategy : NamingStrategyBase {
        protected override bool TransformIfRequired(string name, StringBuilder builder) {
            foreach(var chr in name)
                builder.Append(char.ToUpperInvariant(chr));
            return true;
        }
    }


    public class JsonFormatterSettings : ConverterSettings{
        public static JsonFormatterSettings Idented => new JsonFormatterSettings{Ident = true};
        public static JsonFormatterSettings Default => new JsonFormatterSettings();

        public bool Ident { get; set; }
        public int IdentSize { get; set; } = 4;
    }
}
