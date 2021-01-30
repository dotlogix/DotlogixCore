// ==================================================
// Copyright 2018(C) , DotLogix
// File:  JsonBodyAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.06.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Rest.Services.Parameters;
#endregion

namespace DotLogix.Core.Rest.Services.Attributes {
    [AttributeUsage(AttributeTargets.Parameter)]
    public abstract class ParameterSourceFilterAttribute : Attribute {
        public ParameterSources Sources { get;}
        protected ParameterSourceFilterAttribute(ParameterSources sources = ParameterSources.Any) {
            Sources = sources;
        }

        public virtual bool MatchesProvider(IParameterProvider provider) {
            return (provider.Source & Sources) != 0;
        }
    }
}
