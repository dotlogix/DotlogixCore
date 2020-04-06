// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebRequestProcessorDescriptorCollection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.06.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
#endregion

namespace DotLogix.Core.Rest.Services.Descriptors {
    public class RouteDescriptorCollection : Collection<IRouteDescriptor> {
        public IEnumerable<TDescriptor> GetCustomDescriptors<TDescriptor>() where TDescriptor : IRouteDescriptor {
            return Items.OfType<TDescriptor>();
        }

        public TDescriptor GetCustomDescriptor<TDescriptor>() where TDescriptor : IRouteDescriptor {
            return Items.OfType<TDescriptor>().FirstOrDefault();
        }

        public TDescriptor GetCustomDescriptor<TDescriptor>(string name, StringComparison comparison = StringComparison.Ordinal) where TDescriptor : IRouteDescriptor {
            return Items.OfType<TDescriptor>().FirstOrDefault(d => string.Equals(d.Name, name, comparison));
        }

        public IRouteDescriptor GetCustomDescriptor(string name, StringComparison comparison = StringComparison.Ordinal) {
            return Items.FirstOrDefault(d => string.Equals(d.Name, name, comparison));
        }
    }
}
