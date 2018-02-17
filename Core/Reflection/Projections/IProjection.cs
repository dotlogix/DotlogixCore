﻿// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IProjection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

namespace DotLogix.Core.Reflection.Projections {
    public interface IProjection {
        void ProjectLeftToRight(object left, object right);
        void ProjectRightToLeft(object left, object right);
    }
}
