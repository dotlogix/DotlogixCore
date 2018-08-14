// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Singletons.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  27.04.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Core.Patterns {
    public class Singleton<T> where T : class, new() {
        private static T _instance;

        public static T Instance => _instance ?? (_instance = new T());
    }
}
