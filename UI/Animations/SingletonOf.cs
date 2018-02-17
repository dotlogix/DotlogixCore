// ==================================================
// Copyright 2018(C) , DotLogix
// File:  SingletonOf.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
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
        private static readonly T InstanceBack = new T();

        /// <summary>
        ///     Creates domain-wide singleton of specified type. Warning: Don't use it to initialize static member of the same type
        ///     as a containing class. Use Getter for this.
        /// </summary>
        public static T Instance {
            get {
                Debug.Assert(InstanceBack != null,
                             "SingletonOf<" + typeof(T) +
                             "> : Attempt to initialize static member of the same class. Member will be initialized to null");
                return InstanceBack;
            }
        }
    }
}
