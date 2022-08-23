using System;
using System.Reflection;
using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Core.Utils.Factories {
    /// <summary>
    /// A static class to create instantiators
    /// </summary>
    public static class Factories {
        /// <summary>
        /// Create an instantiator with a singleton property
        /// </summary>
        public static IFactory UseSingletonProperty(Type singletonType, string propertyName = "Instance", Type constraintType = null) {
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

            if(constraintType is not null && targetProperty.PropertyType.IsAssignableTo(constraintType) == false)
                throw new ArgumentException($"The property {targetProperty} of type {singletonType.GetFriendlyName()} is not assignable to constraint {constraintType.GetFriendlyName()}", nameof(constraintType));

            return new SingletonFactory(targetProperty.CreateDynamicProperty());
        }
        /// <summary>
        /// Create an instantiator with a singleton property
        /// </summary>
        public static IFactory<T> UseSingletonProperty<T>(string propertyName = "Instance", Type constraintType = null) {
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

            if(constraintType is not null && targetProperty.PropertyType.IsAssignableTo(constraintType) == false)
                throw new ArgumentException($"The property {targetProperty} of type {singletonType.GetFriendlyName()} is not assignable to constraint {constraintType.GetFriendlyName()}", nameof(constraintType));

            return new SingletonFactory<T>(targetProperty.CreateDynamicProperty());
        }
        /// <summary>
        /// Create an instantiator using the default constructor
        /// </summary>
        public static IFactory UseDefaultCtor(Type type, Type constraintType = null)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (type.IsAbstract)
                throw new ArgumentException($"The type {type.GetFriendlyName()} can not be an abstract class", nameof(type));
            if (type.IsInterface)
                throw new ArgumentException($"The type {type.GetFriendlyName()} can not be an interface", nameof(type));
            if (type.IsGenericTypeDefinition)
                throw new ArgumentException($"The type {type.GetFriendlyName()} can not be an open generic type", nameof(type));

            if (constraintType is not null && type.IsAssignableTo(constraintType) == false)
                throw new ArgumentException($"The type {type.GetFriendlyName()} is not assignable to constraint {constraintType.GetFriendlyName()}", nameof(constraintType));

            var targetCtor = type.CreateDefaultCtor();
            if (targetCtor == null)
                throw new ArgumentException($"The type {type.GetFriendlyName()} does not define a default constructor", nameof(type));

            return new DynamicFactory(targetCtor);
        }
        
        /// <summary>
        /// Create an instantiator using the default constructor
        /// </summary>
        public static IFactory<T> UseDefaultCtor<T>(Type constraintType = null) {
            var type = typeof(T);

            if (type.IsAbstract)
                throw new ArgumentException($"The type {type.GetFriendlyName()} can not be an abstract class", nameof(type));
            if (type.IsInterface)
                throw new ArgumentException($"The type {type.GetFriendlyName()} can not be an interface", nameof(type));
            if (type.IsGenericTypeDefinition)
                throw new ArgumentException($"The type {type.GetFriendlyName()} can not be an open generic type", nameof(type));

            if (constraintType is not null && type.IsAssignableTo(constraintType) == false)
                throw new ArgumentException($"The type {type.GetFriendlyName()} is not assignable to constraint {constraintType.GetFriendlyName()}", nameof(constraintType));

            var targetCtor = type.CreateDefaultCtor();
            if (targetCtor == null)
                throw new ArgumentException($"The type {type.GetFriendlyName()} does not define a default constructor", nameof(type));

            return new DynamicFactory<T>(targetCtor);
        }
        /// <summary>
        /// Create an instantiator using a delegate
        /// </summary>
        public static IFactory UseDelegate(Func<object> instantiateFunc)
        {
            return new DelegateFactory(instantiateFunc);
        }
        /// <summary>
        /// Create an instantiator using a delegate
        /// </summary>
        public static IFactory<T> UseDelegate<T>(Func<T> instantiateFunc)
        {
            return new DelegateFactory<T>(instantiateFunc);
        }

        /// <summary>
        /// Create an instantiator using a delegate
        /// </summary>
        public static IArgsFactory UseDelegate(Func<object[], object> instantiateFunc)
        {
            return new DelegateArgsFactory(instantiateFunc);
        }

        /// <summary>
        /// Create an instantiator using a delegate
        /// </summary>
        public static IArgsFactory<T> UseDelegate<T>(Func<object[], T> instantiateFunc)
        {
            return new DelegateArgsFactory<T>(instantiateFunc);
        }
        /// <summary>
        /// Create an instantiator using a constructor
        /// </summary>
        public static IArgsFactory UseCtor(Type type, Type[] parameterTypes, Type constraintType = null)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (type.IsAbstract)
                throw new ArgumentException($"The type {type.GetFriendlyName()} can not be an abstract class", nameof(type));
            if (type.IsInterface)
                throw new ArgumentException($"The type {type.GetFriendlyName()} can not be an interface", nameof(type));
            if (type.IsGenericTypeDefinition)
                throw new ArgumentException($"The type {type.GetFriendlyName()} can not be an open generic type", nameof(type));

            if (constraintType is not null && type.IsAssignableTo(constraintType) == false)
                throw new ArgumentException($"The type {type.GetFriendlyName()} is not assignable to constraint {constraintType.GetFriendlyName()}", nameof(constraintType));

            var targetCtor = type.CreateDynamicCtor(parameterTypes);
            if (targetCtor == null)
                throw new ArgumentException($"The type {type.GetFriendlyName()} does not define a default constructor", nameof(type));

            return new DynamicArgsFactory(targetCtor);
        }

        /// <summary>
        /// Create an instantiator using a constructor
        /// </summary>
        public static IArgsFactory<T> UseCtor<T>(Type[] parameterTypes, Type constraintType = null) {
            var type = typeof(T);

            if (type.IsAbstract)
                throw new ArgumentException($"The type {type.GetFriendlyName()} can not be an abstract class", nameof(type));
            if (type.IsInterface)
                throw new ArgumentException($"The type {type.GetFriendlyName()} can not be an interface", nameof(type));
            if (type.IsGenericTypeDefinition)
                throw new ArgumentException($"The type {type.GetFriendlyName()} can not be an open generic type", nameof(type));

            if (constraintType is not null && type.IsAssignableTo(constraintType) == false)
                throw new ArgumentException($"The type {type.GetFriendlyName()} is not assignable to constraint {constraintType.GetFriendlyName()}", nameof(constraintType));

            var targetCtor = type.CreateDynamicCtor(parameterTypes);
            if (targetCtor == null)
                throw new ArgumentException($"The type {type.GetFriendlyName()} does not define a default constructor", nameof(type));

            return new DynamicArgsFactory<T>(targetCtor);
        }
    }
}