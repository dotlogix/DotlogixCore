// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ILayeredCollection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.12.2018
// LastEdited:  13.12.2018
// ==================================================

#region
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Collections {
    public interface ILayeredCollection<TValue, TCollection> : ICollection<TValue> where TCollection : ICollection<TValue> {
        IEnumerable<TCollection> Layers { get; }
        TCollection CurrentLayer { get; }

        void PushLayer(TCollection collection);
        TCollection PopLayer();
        TCollection PeekLayer();
    }
}
