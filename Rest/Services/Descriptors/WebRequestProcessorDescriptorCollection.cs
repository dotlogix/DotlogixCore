using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DotLogix.Core.Rest.Services.Descriptors {
    public class WebRequestProcessorDescriptorCollection : Collection<IWebRequestProcessorDescriptor> {
        public IEnumerable<TDescriptor> GetCustomDescriptors<TDescriptor>() where TDescriptor:IWebRequestProcessorDescriptor{
            return Items.OfType<TDescriptor>();
        }

        public TDescriptor GetCustomDescriptor<TDescriptor>() where TDescriptor : IWebRequestProcessorDescriptor
        {
            return Items.OfType<TDescriptor>().FirstOrDefault();
        }

        public TDescriptor GetCustomDescriptor<TDescriptor>(string name, StringComparison comparison = StringComparison.Ordinal) where TDescriptor : IWebRequestProcessorDescriptor
        {
            return Items.OfType<TDescriptor>().FirstOrDefault(d=> string.Equals(d.Name, name, comparison));
        }

        public IWebRequestProcessorDescriptor GetCustomDescriptor(string name, StringComparison comparison = StringComparison.Ordinal)
        {
            return Items.FirstOrDefault(d => string.Equals(d.Name, name, comparison));
        }
    }
}