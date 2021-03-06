﻿// ==================================================
// Copyright 2018(C) , DotLogix
// File:  INodeConverterFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Core.Nodes.Converters;
#endregion

namespace DotLogix.Core.Nodes.Factories {
    /// <summary>
    /// An interface representing a factory to create node converters
    /// </summary>
    public interface INodeConverterFactory {
        /// <summary>
        /// Create a new node converter
        /// </summary>
        IAsyncNodeConverter CreateConverter(INodeConverterResolver resolver, TypeSettings typeSettings);
        /// <summary>
        /// Try to create a new node converter
        /// </summary>
        bool TryCreateConverter(INodeConverterResolver resolver, TypeSettings typeSettings, out IAsyncNodeConverter converter);
    }
}
