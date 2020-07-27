using System;
using System.Reflection;
using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Utils.Instantiators;

namespace DotLogix.Core.Utils
{
    /// <summary>
    /// A static class to create instantiators
    /// </summary>
    public static class Instantiator {
        /// <summary>
        /// Create an instantiator with a singleton property
        /// </summary>
        public static IInstantiator UseSingletonProperty(Type singletonType, string propertyName = "Instance", Type constraintType = null) {
            if(singletonType == null)
                throw new ArgumentNullException(nameof(singletonType));
            if(propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));

            if (singletonType.IsInterface)
                throw new ArgumentException($"The type {singletonType.GetFriendlyName()} can not be an interface", nameof(singletonType));
            if (singletonType.IsGenericTypeDefinition)
                throw new ArgumentException($"The type {singletonType.GetFriendlyName()} can not be an open generic type", nameof(singletonType));

            var targetProperty = singletonType.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Static);
            if(targetProperty == null)
                throw new ArgumentException($"The type {singletonType.GetFriendlyName()} does not define a static property {propertyName}", nameof(singletonType));

            if(constraintType != null && targetProperty.PropertyType.IsAssignableTo(constraintType) == false)
                throw new ArgumentException($"The property {targetProperty} of type {singletonType.GetFriendlyName()} is not assignable to constraint {constraintType.GetFriendlyName()}", nameof(constraintType));

            return new SingletonInstantiator(targetProperty.CreateDynamicProperty());
        }
        /// <summary>
        /// Create an instantiator with a singleton property
        /// </summary>
        public static IInstantiator<T> UseSingletonProperty<T>(string propertyName = "Instance", Type constraintType = null) {
            var singletonType = typeof(T);
            if(propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));

            if (singletonType.IsInterface)
                throw new ArgumentException($"The type {singletonType.GetFriendlyName()} can not be an interface", nameof(singletonType));
            if (singletonType.IsGenericTypeDefinition)
                throw new ArgumentException($"The type {singletonType.GetFriendlyName()} can not be an open generic type", nameof(singletonType));

            var targetProperty = singletonType.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Static);
            if(targetProperty == null)
                throw new ArgumentException($"The type {singletonType.GetFriendlyName()} does not define a static property {propertyName}", nameof(singletonType));

            if(constraintType != null && targetProperty.PropertyType.IsAssignableTo(constraintType) == false)
                throw new ArgumentException($"The property {targetProperty} of type {singletonType.GetFriendlyName()} is not assignable to constraint {constraintType.GetFriendlyName()}", nameof(constraintType));

            return new SingletonInstantiator<T>(targetProperty.CreateDynamicProperty());
        }
        /// <summary>
        /// Create an instantiator using the default constructor
        /// </summary>
        public static IInstantiator UseDefaultCtor(Type type, Type constraintType = null)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (type.IsAbstract)
                throw new ArgumentException($"The type {type.GetFriendlyName()} can not be an abstract class", nameof(type));
            if (type.IsInterface)
                throw new ArgumentException($"The type {type.GetFriendlyName()} can not be an interface", nameof(type));
            if (type.IsGenericTypeDefinition)
                throw new ArgumentException($"The type {type.GetFriendlyName()} can not be an open generic type", nameof(type));

            if (constraintType != null && type.IsAssignableTo(constraintType) == false)
                throw new ArgumentException($"The type {type.GetFriendlyName()} is not assignable to constraint {constraintType.GetFriendlyName()}", nameof(constraintType));

            var targetCtor = type.CreateDefaultCtor();
            if (targetCtor == null)
                throw new ArgumentException($"The type {type.GetFriendlyName()} does not define a default constructor", nameof(type));

            return new DynamicInstantiator(targetCtor);
        }
        
        /// <summary>
        /// Create an instantiator using the default constructor
        /// </summary>
        public static IInstantiator<T> UseDefaultCtor<T>(Type constraintType = null) {
            var type = typeof(T);

            if (type.IsAbstract)
                throw new ArgumentException($"The type {type.GetFriendlyName()} can not be an abstract class", nameof(type));
            if (type.IsInterface)
                throw new ArgumentException($"The type {type.GetFriendlyName()} can not be an interface", nameof(type));
            if (type.IsGenericTypeDefinition)
                throw new ArgumentException($"The type {type.GetFriendlyName()} can not be an open generic type", nameof(type));

            if (constraintType != null && type.IsAssignableTo(constraintType) == false)
                throw new ArgumentException($"The type {type.GetFriendlyName()} is not assignable to constraint {constraintType.GetFriendlyName()}", nameof(constraintType));

            var targetCtor = type.CreateDefaultCtor();
            if (targetCtor == null)
                throw new ArgumentException($"The type {type.GetFriendlyName()} does not define a default constructor", nameof(type));

            return new DynamicInstantiator<T>(targetCtor);
        }
        /// <summary>
        /// Create an instantiator using a delegate
        /// </summary>
        public static IInstantiator UseDelegate(Func<object> instantiateFunc)
        {
            return new DelegateInstantiator(instantiateFunc);
        }
        /// <summary>
        /// Create an instantiator using a delegate
        /// </summary>
        public static IInstantiator<T> UseDelegate<T>(Func<T> instantiateFunc)
        {
            return new DelegateInstantiator<T>(instantiateFunc);
        }

        /// <summary>
        /// Create an instantiator using a delegate
        /// </summary>
        public static IArgsInstantiator UseDelegate(Func<object[], object> instantiateFunc)
        {
            return new DelegateArgsInstantiator(instantiateFunc);
        }

        /// <summary>
        /// Create an instantiator using a delegate
        /// </summary>
        public static IArgsInstantiator<T> UseDelegate<T>(Func<object[], T> instantiateFunc)
        {
            return new DelegateArgsInstantiator<T>(instantiateFunc);
        }
        /// <summary>
        /// Create an instantiator using a constructor
        /// </summary>
        public static IArgsInstantiator UseCtor(Type type, Type[] parameterTypes, Type constraintType = null)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (type.IsAbstract)
                throw new ArgumentException($"The type {type.GetFriendlyName()} can not be an abstract class", nameof(type));
            if (type.IsInterface)
                throw new ArgumentException($"The type {type.GetFriendlyName()} can not be an interface", nameof(type));
            if (type.IsGenericTypeDefinition)
                throw new ArgumentException($"The type {type.GetFriendlyName()} can not be an open generic type", nameof(type));

            if (constraintType != null && type.IsAssignableTo(constraintType) == false)
                throw new ArgumentException($"The type {type.GetFriendlyName()} is not assignable to constraint {constraintType.GetFriendlyName()}", nameof(constraintType));

            var targetCtor = type.CreateDynamicCtor(parameterTypes);
            if (targetCtor == null)
                throw new ArgumentException($"The type {type.GetFriendlyName()} does not define a default constructor", nameof(type));

            return new DynamicArgsInstantiator(targetCtor);
        }

        /// <summary>
        /// Create an instantiator using a constructor
        /// </summary>
        public static IArgsInstantiator<T> UseCtor<T>(Type[] parameterTypes, Type constraintType = null) {
            var type = typeof(T);

            if (type.IsAbstract)
                throw new ArgumentException($"The type {type.GetFriendlyName()} can not be an abstract class", nameof(type));
            if (type.IsInterface)
                throw new ArgumentException($"The type {type.GetFriendlyName()} can not be an interface", nameof(type));
            if (type.IsGenericTypeDefinition)
                throw new ArgumentException($"The type {type.GetFriendlyName()} can not be an open generic type", nameof(type));

            if (constraintType != null && type.IsAssignableTo(constraintType) == false)
                throw new ArgumentException($"The type {type.GetFriendlyName()} is not assignable to constraint {constraintType.GetFriendlyName()}", nameof(constraintType));

            var targetCtor = type.CreateDynamicCtor(parameterTypes);
            if (targetCtor == null)
                throw new ArgumentException($"The type {type.GetFriendlyName()} does not define a default constructor", nameof(type));

            return new DynamicArgsInstantiator<T>(targetCtor);
        }
    }
}
