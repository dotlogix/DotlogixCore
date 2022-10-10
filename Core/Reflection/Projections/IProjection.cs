// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IProjection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Core.Reflection.Projections {
    /// <summary>
    /// An interface for object to object projections
    /// </summary>
    public interface IProjection {
        /// <summary>
        /// Project the left object to the right
        /// </summary>
        void ProjectLeftToRight(object left, object right);
        /// <summary>
        /// Project the right object to the left
        /// </summary>
        void ProjectRightToLeft(object left, object right);
    }
}
