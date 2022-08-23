// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ObservableCollectionExtension.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.ObjectModel;
#endregion

namespace DotLogix.Core.Extensions {
    /// <summary>
    /// A static class providing extension methods for <see cref="ObservableCollection{T}"/>
    /// </summary>
    public static class ObservableCollectionExtension {
        /// <summary>
        ///     Converts a observable collection to a read-only one
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static ReadOnlyObservableCollection<T> AsReadonly<T>(this ObservableCollection<T> collection) {
            return new ReadOnlyObservableCollection<T>(collection);
        }
    }
}