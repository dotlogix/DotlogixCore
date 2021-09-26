// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  LambdaBuilders.String.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 07.06.2021 19:19
// LastEdited:  26.09.2021 22:15
// ==================================================

using System.Text.RegularExpressions;

namespace DotLogix.Core.Expressions {
    public static partial class LambdaBuilders {
        private const string Dummy = "";

        public static LambdaBuilder<bool> MatchesRegex(this LambdaBuilder<string> instance, LambdaBuilder<string> pattern, LambdaBuilder<RegexOptions> options = null) {
            return options != null
                       ? instance.CallStatic(Regex.IsMatch, pattern, options)
                       : instance.CallStatic(Regex.IsMatch, pattern);
        }
        
        public static LambdaBuilder<string> ToLower(this LambdaBuilder<string> instance) {
            return instance.Call(Dummy.ToLower);
        }
        public static LambdaBuilder<string> ToUpper(this LambdaBuilder<string> instance) {
            return instance.Call(Dummy.ToUpper);
        }
        
        public static LambdaBuilder<bool> StartsWith(this LambdaBuilder<string> instance, LambdaBuilder<string> other) {
            return instance.Call(Dummy.StartsWith, other);
        }
        public static LambdaBuilder<bool> EndsWith(this LambdaBuilder<string> instance, LambdaBuilder<string> other) {
            return instance.Call(Dummy.EndsWith, other);
        }
        public static LambdaBuilder<bool> Contains(this LambdaBuilder<string> instance, LambdaBuilder<string> other) {
            return instance.Call(Dummy.Contains, other);
        }
        public static LambdaBuilder<int> IndexOf(this LambdaBuilder<string> instance, LambdaBuilder<string> other) {
            return instance.Call(Dummy.IndexOf, other);
        }
        public static LambdaBuilder<string> Concat(this LambdaBuilder<string> instance, LambdaBuilder<string> other) {
            return instance.CallStatic(string.Concat, other);
        }
        public static LambdaBuilder<string> Concat(this LambdaBuilder<string> instance, LambdaBuilder<string[]> other) {
            return instance.Call(string.Concat, other);
        }
        public static LambdaBuilder<string> Substring(this LambdaBuilder<string> instance, LambdaBuilder<int> index, LambdaBuilder<int> count) {
            return instance.Call(Dummy.Substring, index, count);
        }
#if NETSTANDARD2_1
        public static LambdaBuilder<string> Trim(this LambdaBuilder<string> instance, LambdaBuilder<char> character = null) {
            return instance.Call(Dummy.Trim, character);
        }
        public static LambdaBuilder<string> TrimStart(this LambdaBuilder<string> instance, LambdaBuilder<char> character = null) {
            return instance.Call(Dummy.TrimStart, character);
        }
        public static LambdaBuilder<string> TrimEnd(this LambdaBuilder<string> instance, LambdaBuilder<char> character = null) {
            return instance.Call(Dummy.TrimEnd, character);
        }
#else
        public static LambdaBuilder<string> Trim(this LambdaBuilder<string> instance, LambdaBuilder<char[]> character = null) {
            return instance.Call(Dummy.Trim, character);
        }
        public static LambdaBuilder<string> TrimStart(this LambdaBuilder<string> instance, LambdaBuilder<char[]> character = null) {
            return instance.Call(Dummy.TrimStart, character);
        }
        public static LambdaBuilder<string> TrimEnd(this LambdaBuilder<string> instance, LambdaBuilder<char[]> character = null) {
            return instance.Call(Dummy.TrimEnd, character);
        }
#endif
        
    }
}