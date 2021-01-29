// ==================================================
// Copyright 2020(C) , DotLogix
// File:  NodeContainerStack.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  ..
// LastEdited:  27.04.2020
// ==================================================

using System;
using System.Collections.Generic;

namespace DotLogix.Core.Nodes.Formats
{
    public class NodeContainerStack : Stack<NodeContainerType> {
        public NodeContainerType Current => Count > 0 ? Peek() : NodeContainerType.None;

        public void PopExpected(NodeContainerType expectedType)
        {
            if (Count == 0)
                throw new InvalidOperationException("There is nothing on the container stack");

            var currentContainer = Pop();
            if (currentContainer == expectedType)
                return;

            Push(currentContainer);
            throw new InvalidOperationException($"The current container doesn't match the expected type {expectedType} (current: {currentContainer})");
        }
    }
}