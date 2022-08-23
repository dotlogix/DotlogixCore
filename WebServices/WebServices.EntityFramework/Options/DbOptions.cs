// ==================================================
// Copyright 2019(C) , DotLogix
// File:  DbConfiguration.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  16.07.2018
// LastEdited:  20.01.2019
// ==================================================

namespace DotLogix.WebServices.EntityFramework.Options; 

public class DbOptions {
    public string ConnectionString { get; set; }
    public string Password { get; set; }
    public int? MaxRetries { get; set; }
    public int? CommandTimeout { get; set; }
}