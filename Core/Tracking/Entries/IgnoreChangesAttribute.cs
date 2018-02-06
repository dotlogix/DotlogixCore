using System;

namespace DotLogix.Core.Tracking.Entries {
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreChangesAttribute : Attribute
    {
    }
}