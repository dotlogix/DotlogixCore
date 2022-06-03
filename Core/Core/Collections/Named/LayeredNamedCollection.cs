// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  LayeredNamedCollection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 22.08.2020 13:51
// LastEdited:  26.09.2021 22:27
// ==================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace DotLogix.Core.Collections {
    public class LayeredNamedCollection : CascadingNamedCollection, ILayeredNamedCollection {
        /// <summary>
        /// The source settings stack
        /// </summary>
        protected new Stack<IReadOnlyNamedCollection> Values { get; }

        /// <inheritdoc />
        public IEnumerable<IReadOnlyNamedCollection> Layers => Values;

        /// <inheritdoc />
        public IReadOnlyNamedCollection CurrentLayer => PeekLayer();

        /// <inheritdoc />
        public LayeredNamedCollection() : this(default(IEqualityComparer<string>)) {
            
        }
        
        /// <inheritdoc />
        public LayeredNamedCollection(IEqualityComparer<string> nameComparer) : base(new Stack<IReadOnlyNamedCollection>(), nameComparer ?? StringComparer.Ordinal) {
            Values = (Stack<IReadOnlyNamedCollection>)base.Values;
        }
        
        /// <inheritdoc />
        public LayeredNamedCollection(params IReadOnlyNamedCollection[] initialStack) : this(initialStack.AsEnumerable()) {
        }
        
        /// <inheritdoc />
        public LayeredNamedCollection(IEnumerable<IReadOnlyNamedCollection> initialStack, IEqualityComparer<string> nameComparer = null) : base(new Stack<IReadOnlyNamedCollection>(initialStack), nameComparer ?? StringComparer.Ordinal) {
            Values = (Stack<IReadOnlyNamedCollection>)base.Values;
        }

        /// <inheritdoc />
        public override IReadOnlyNamedCollection Clone() {
            return new LayeredNamedCollection(Values, NameComparer);
        }


        /// <inheritdoc />
        public INamedCollection PushLayer() {
            var settings = new NamedCollection(NameComparer);
            Values.Push(settings);
            return settings;
        }

        /// <inheritdoc />
        public virtual IReadOnlyNamedCollection PopLayer() {
            return Values.Pop();
        }

        /// <inheritdoc />
        public virtual IReadOnlyNamedCollection PeekLayer() {
            return Values.Peek();
        }
    }
}