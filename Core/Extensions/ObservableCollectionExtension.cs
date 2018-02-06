// ==================================================
// Copyright 2016(C) , DotLogix
// File:  ObservableCollectionExtension.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  06.09.2017
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
