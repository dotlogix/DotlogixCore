// ==================================================
// Copyright 2018(C) , DotLogix
// File:  InvokeDelegate.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Core.Reflection.Delegates; 

/// <summary>
/// A delegate to represent method calls
/// </summary>
public delegate object InvokeDelegate(object instance, object[] parameters);