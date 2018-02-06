// ==================================================
// Copyright 2016(C) , DotLogix
// File:  SingletonOf.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  23.06.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System.Diagnostics;
#endregion

namespace DotLogix.UI.Animations {
    /// <summary>
    ///     Creates application-wide singleton of specified type.
    /// </summary>
    /// <remarks>
    ///     Type should have public default constructor. Warning: Don't use it to initialize static member of the same type as
    ///     a containing class. Use Getter for this.
    /// </remarks>
    /// <typeparam name="T"> class </typeparam>
    public static class SingletonOf<T> where T : class, new() {
        private static readonly T _instance = new T();

        /// <summary>
        ///     Creates domain-wide singleton of specified type. Warning: Don't use it to initialize static member of the same type
        ///     as a containing class. Use Getter for this.
        /// </summary>
        public static T Instance {
            get {
                Debug.Assert(_instance != null,
                             "SingletonOf<" + typeof(T) +
                             "> : Attempt to initialize static member of the same class. Member will be initialized to null");
                return _instance;
            }
        }
    }
}
