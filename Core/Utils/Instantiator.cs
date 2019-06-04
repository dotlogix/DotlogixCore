using System;
using System.Reflection;
using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Utils.Instantiators;

namespace DotLogix.Core.Utils
{
    public static class Instantiator {


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
        public static IInstantiator UseDelegate(Func<object> instantiateFunc)
        {
            return new DelegateInstantiator(instantiateFunc);
        }
        public static IArgsInstantiator UseDelegate(Func<object[], object> instantiateFunc)
        {
            return new DelegateArgsInstantiator(instantiateFunc);
        }
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
    }
}
