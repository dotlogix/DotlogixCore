// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DynamicType.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Reflection.Dynamics {
    /// <summary>
    /// A representation of a type
    /// </summary>
    public class DynamicType {
        private const BindingFlags AllBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                                                     BindingFlags.NonPublic;

        private readonly IReadOnlyDictionary<string, DynamicAccessor> _accessorsDict;


        private readonly DynamicCtor _defaultCtor;
        private readonly IReadOnlyDictionary<string, DynamicField> _fieldDict;
        private readonly IReadOnlyDictionary<string, DynamicProperty> _propertiesDict;
        /// <summary>
        /// The original type
        /// </summary>
        public Type Type { get; }
        /// <summary>
        /// The name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The constructors
        /// </summary>
        public IReadOnlyList<DynamicCtor> Constructors { get; }
        /// <summary>
        /// The methods
        /// </summary>
        public IReadOnlyList<DynamicInvoke> Methods { get; }

        /// <summary>
        /// The accessors
        /// </summary>
        public IReadOnlyList<DynamicAccessor> Accessors { get; }

        /// <summary>
        /// The properties
        /// </summary>
        public IReadOnlyList<DynamicProperty> Properties { get; }

        /// <summary>
        /// The fields
        /// </summary>
        public IReadOnlyList<DynamicField> Fields { get; }

        /// <summary>
        /// Checks if there is a default constructor
        /// </summary>
        public bool HasDefaultConstructor => _defaultCtor != null;

        /// <summary>
        /// Creates an instance of <see cref="DynamicType"/>
        /// </summary>
        public DynamicType(Type type, MemberTypes includedMemberTypes = MemberTypes.All) {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Name = type.Name;

            Fields = CreateFields(type, includedMemberTypes)?.ToList();
            Properties = CreateProperties(type, includedMemberTypes)?.ToList();
            Accessors = CreateAccessors(Fields, Properties, includedMemberTypes)?.ToList();
            Methods = CreateMethods(type, includedMemberTypes)?.ToList();
            Constructors = CreateConstructors(type, out _defaultCtor, includedMemberTypes);

            _propertiesDict = Properties?.ToDictionary(p => p.Name);
            _fieldDict = Fields?.ToDictionary(f => f.Name);
            _accessorsDict = Accessors?.ToDictionary(a => a.Name);
        }

        #region Object
        /// <summary>
        /// Returns the name of the type
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return Name;
        }
        #endregion

        #region Accessors
        /// <summary>
        /// Tries to get a property.<br></br>
        /// If the property can not be found the method returns null
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public DynamicProperty GetPropery(string propertyName) {
            if(propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));
            return _propertiesDict.TryGetValue(propertyName, out var property) ? property : null;
        }

        /// <summary>
        /// Tries to get a field.<br></br>
        /// If the field can not be found the method returns null
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public DynamicField GetField(string fieldName) {
            if(fieldName == null)
                throw new ArgumentNullException(nameof(fieldName));
            return _fieldDict.TryGetValue(fieldName, out var field) ? field : null;
        }

        /// <summary>
        /// Tries to get a accessor.<br></br>
        /// If the accessor can not be found the method returns null
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public DynamicAccessor GetAccessor(string accessorName) {
            if(accessorName == null)
                throw new ArgumentNullException(nameof(accessorName));
            return _accessorsDict.TryGetValue(accessorName, out var accessor) ? accessor : null;
        }

        /// <summary>
        /// Enumerate all accessors of the type
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public IEnumerable<DynamicAccessor> GetAccessors(AccessorTypes types = AccessorTypes.Any,
                                                         ValueAccessModes accessModes = ValueAccessModes.ReadWrite) {
            IEnumerable<DynamicAccessor> accessors;
            switch(types) {
                case AccessorTypes.None:
                    return Enumerable.Empty<DynamicAccessor>();
                case AccessorTypes.Property:
                    accessors = Properties;
                    break;
                case AccessorTypes.Field:
                    accessors = Fields;
                    break;
                case AccessorTypes.Any:
                    accessors = Accessors;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(types), types, null);
            }
            return accessors.Where(a => (a.ValueAccessMode & accessModes) == accessModes);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Tries to get a method.<br></br>
        /// If the method can not be found the method returns null
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>

        public DynamicInvoke GetMethod(string methodName) {
            if(methodName == null)
                throw new ArgumentNullException(nameof(methodName));
            var methods = GetMethods(methodName).ToList();
            if(methods.Count > 1)
                throw new AmbiguousMatchException("There are multiple methods with this name defined");
            return methods.Count == 1 ? methods[0] : null;
        }

        /// <summary>
        /// Tries to get a method.<br></br>
        /// If the method can not be found the method returns null
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public DynamicInvoke GetMethod(string methodName, params Type[] parametersTypes) {
            if(methodName == null)
                throw new ArgumentNullException(nameof(methodName));
            if(parametersTypes == null)
                throw new ArgumentNullException(nameof(parametersTypes));
            return Methods.FirstOrDefault(m => (m.Name == methodName) &&
                                               TypeArrayEquals(m.ParameterTypes, parametersTypes));
        }

        /// <summary>
        /// Enumerate all methods
        /// </summary>
        public IEnumerable<DynamicInvoke> GetMethods(string methodName) {
            return Methods.Where(m => m.Name == methodName);
        }
        #endregion

        #region Ctor

        /// <summary>
        /// Tries to get a constructor.<br></br>
        /// If the constructor can not be found the method returns null
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>

        public DynamicCtor GetConstructor(params Type[] parametersTypes) {
            if(parametersTypes == null)
                throw new ArgumentNullException(nameof(parametersTypes));
            if(parametersTypes.Length == 0)
                return _defaultCtor;

            return Constructors.FirstOrDefault(c => TypeArrayEquals(c.ParameterTypes, parametersTypes));
        }

        /// <summary>
        /// Tries to get a constructor.<br></br>
        /// If the constructor can not be found the method returns null
        /// </summary>
        public DynamicCtor GetDefaultConstructor() {
            return _defaultCtor;
        }
        #endregion

        #region Helper
        private static List<DynamicCtor> CreateConstructors(Type type, out DynamicCtor defaultCtor,
                                                            MemberTypes includedMemberTypes) {
            if((includedMemberTypes & MemberTypes.Constructor) == 0) {
                defaultCtor = null;
                return new List<DynamicCtor>();
            }

            var dynamicCtors = from constructor in type.GetConstructors(AllBindingFlags)
                               let dynamicCtor = constructor.CreateDynamicCtor()
                               where dynamicCtor != null
                               select dynamicCtor;
            var ctors = dynamicCtors.ToList();
            defaultCtor = ctors.FirstOrDefault(ctor => ctor.IsDefault);
            if((defaultCtor != null) || (type.IsValueType == false))
                return ctors;

            defaultCtor = type.CreateDefaultCtor();
            if(defaultCtor != null)
                ctors.Insert(0, defaultCtor);
            return ctors;
        }

        private static IEnumerable<DynamicInvoke> CreateMethods(Type type, MemberTypes includedMemberTypes) {
            if((includedMemberTypes & MemberTypes.Method) == 0)
                return null;
            var dynamicMethods = from method in type.GetMethods(AllBindingFlags)
                                 let dynamicMethod = method.CreateDynamicInvoke()
                                 where dynamicMethod != null
                                 select dynamicMethod;
            return dynamicMethods;
        }

        private static IEnumerable<DynamicField> CreateFields(Type type, MemberTypes includedMemberTypes) {
            if((includedMemberTypes & MemberTypes.Field) == 0)
                return Enumerable.Empty<DynamicField>();
            var dynamicFields = from field in type.GetFields(AllBindingFlags)
                                let dynamicField = field.CreateDynamicField()
                                where dynamicField != null
                                select dynamicField;
            return dynamicFields;
        }

        private static IEnumerable<DynamicProperty> CreateProperties(Type type, MemberTypes includedMemberTypes) {
            if((includedMemberTypes & MemberTypes.Property) == 0)
                return null;
            var dynamicProperties = from property in type.GetPropertiesByInheritance()
                                    let dynamicProperty = property.CreateDynamicProperty()
                                    where dynamicProperty != null
                                    select dynamicProperty;
            return dynamicProperties;
        }

        private static IEnumerable<DynamicAccessor> CreateAccessors(
            IEnumerable<DynamicAccessor> dynamicFields, IEnumerable<DynamicAccessor> dynamicProperties,
            MemberTypes includedMemberTypes) {
            if(((includedMemberTypes & MemberTypes.Field) | MemberTypes.Property) == 0)
                return null;

            if(dynamicFields == null)
                return dynamicProperties;

            return dynamicProperties != null ? dynamicFields.Concat(dynamicProperties) : dynamicFields;
        }

        private static bool TypeArrayEquals(Type[] left, Type[] right) {
            if(left.Length != right.Length)
                return false;
            for(var i = 0; i < left.Length; i++) {
                if(left[i] != right[i])
                    return false;
            }
            return true;
        }
        #endregion
    }
}
