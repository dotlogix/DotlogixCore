// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebRequestProcessorDescriptorBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.06.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Core.Rest.Services.Descriptors {
    public abstract class WebRequestProcessorDescriptorBase : IWebRequestProcessorDescriptor {
        protected WebRequestProcessorDescriptorBase(string name) {
            Name = name;
        }

        protected WebRequestProcessorDescriptorBase() {
            Name = GetType().Name;
        }

        public string Name { get; }
    }
}
