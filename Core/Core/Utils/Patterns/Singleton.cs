// ==================================================
// Copyright 2019(C) , DotLogix
// File:  Singleton.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  15.08.2018
// LastEdited:  06.02.2019
// ==================================================

namespace DotLogix.Core.Utils.Patterns; 

/// <summary>
/// A generic trick to create typed singletons
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> where T : class, new() {
    private static T _instance;

    /// <summary>
    /// The instance
    /// </summary>
    public static T Instance => _instance ??= new T();
}