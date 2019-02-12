// ==================================================
// Copyright 2019(C) , DotLogix
// File:  Singleton.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  15.08.2018
// LastEdited:  06.02.2019
// ==================================================

namespace DotLogix.Core.Patterns {
    public class Singleton<T> where T : class, new() {
        private static T _instance;

        public static T Instance => _instance ?? (_instance = new T());
    }
}
