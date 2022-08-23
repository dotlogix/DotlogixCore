using System;

namespace DotLogix.WebServices.Core; 

[Flags]
public enum CacheOptions {
    None = 0,
    Local = 1 << 0,
    Global = 1 << 1,
    All = Local|Global,
}