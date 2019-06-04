using System;

namespace DotLogix.Core.Utils {
    public interface IInstantiator {
        object GetInstance();
    }
    
    public interface IArgsInstantiator {
        object GetInstance(params object[] args);
    }
}