// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ObservableCollectionExtension.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Collections.ObjectModel;
#endregion

namespace DotLogix.Core.Extensions {
    public static class ObservableCollectionExtension {
        public static ReadOnlyObservableCollection<T> AsReadonly<T>(this ObservableCollection<T> collection) {
            return new ReadOnlyObservableCollection<T>(collection);
        }
    }
}
