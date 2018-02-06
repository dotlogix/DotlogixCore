// ==================================================
// Copyright 2016(C) , DotLogix
// File:  DynamicType.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#endregion

namespace DotLogix.Core.Reflection.Dynamics {
    public class DynamicType {
        private const BindingFlags AllBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                                                     BindingFlags.NonPublic;

        private readonly Dictionary<string, DynamicAccessor> _accessors;
        private readonly List<DynamicCtor> _constructors;
        private readonly DynamicCtor _defaultCtor;
        private readonly Dictionary<string, DynamicField> _fields;
        private readonly List<DynamicInvoke> _methods;
        private readonly Dictionary<string, DynamicProperty> _properties;
        public Type Type { get; }
        public string Name { get; }
        public IEnumerable<DynamicCtor> Constructors => _constructors;
        public IEnumerable<DynamicInvoke> Methods => _methods;
        public IEnumerable<DynamicAccessor> Accessors => _accessors.Values;
        public IEnumerable<DynamicProperty> Properties => _properties.Values;
        public IEnumerable<DynamicField> Fields => _fields.Values;
        public bool HasDefaultConstructor => _defaultCtor != null;

        public DynamicType(Type type, MemberTypes includedMemberTypes = MemberTypes.All) {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Name = type.Name;

            _fields = CreateFields(type, includedMemberTypes);
            _properties = CreateProperties(type, includedMemberTypes);
            _accessors = CreateAccessors(_fields.Values, _properties.Values, includedMemberTypes);
            _methods = CreateMethods(type, includedMemberTypes);
            _constructors = CreateConstructors(type, out _defaultCtor, includedMemberTypes);
        }

        #region Object
        public override string ToString() {
            return Name;
        }
        #endregion

        #region Accessors
        public DynamicProperty GetPropery(string propertyName) {
            if(propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));
            return _properties.TryGetValue(propertyName, out var property) ? property : null;
        }

        public DynamicField GetField(string fieldName) {
            if(fieldName == null)
                throw new ArgumentNullException(nameof(fieldName));
            return _fields.TryGetValue(fieldName, out var field) ? field : null;
        }

        public DynamicAccessor GetAccessor(string accessorName) {
            if(accessorName == null)
                throw new ArgumentNullException(nameof(accessorName));
            return _accessors.TryGetValue(accessorName, out var accessor) ? accessor : null;
        }

        public IEnumerable<DynamicAccessor> GetAccessors(AccessorTypes types = AccessorTypes.Any,
                                                         ValueAccessModes accessModes = ValueAccessModes.ReadWrite) {
            IEnumerable<DynamicAccessor> accessors;
            switch(types) {
                case AccessorTypes.None:
                    return Enumerable.Empty<DynamicAccessor>();
                case AccessorTypes.Property:
                    accessors = _properties.Values;
                    break;
                case AccessorTypes.Field:
                    accessors = _fields.Values;
                    break;
                case AccessorTypes.Any:
                    accessors = _accessors.Values;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(types), types, null);
            }
            return accessors.Where(a => (a.ValueAccessMode & accessModes) == accessModes);
        }
        #endregion

        #region Methods
        public DynamicInvoke GetMethod(string methodName) {
            if(methodName == null)
                throw new ArgumentNullException(nameof(methodName));
            var methods = GetMethods(methodName).ToList();
            if(methods.Count > 1)
                throw new AmbiguousMatchException("There are multiple methods with this name defined");
            return methods.Count == 1 ? methods[0] : null;
        }

        public DynamicInvoke GetMethod(string methodName, params Type[] parametersTypes) {
            if(methodName == null)
                throw new ArgumentNullException(nameof(methodName));
            if(parametersTypes == null)
                throw new ArgumentNullException(nameof(parametersTypes));
            return _methods.FirstOrDefault(m => (m.Name == methodName) &&
                                                TypeArrayEquals(m.ParameterTypes, parametersTypes));
        }

        public IEnumerable<DynamicInvoke> GetMethods(string methodName) {
            return _methods.Where(m => m.Name == methodName);
        }
        #endregion

        #region Ctor
        public DynamicCtor GetConstructor(params Type[] parametersTypes) {
            if(parametersTypes == null)
                throw new ArgumentNullException(nameof(parametersTypes));
            if(parametersTypes.Length == 0)
                return _defaultCtor;
            return _constructors.FirstOrDefault(c => TypeArrayEquals(c.ParameterTypes, parametersTypes));
        }

        public DynamicCtor GetDefaultConstructor() {
            return _defaultCtor;
        }
        #endregion

        #region Helper
        private static List<DynamicCtor> CreateConstructors(Type type, out DynamicCtor defaultCtor,
                                                            MemberTypes includedMemberTypes) {
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

        private static List<DynamicInvoke> CreateMethods(Type type, MemberTypes includedMemberTypes) {
            if((includedMemberTypes & MemberTypes.Method) == 0)
                return new List<DynamicInvoke>();
            var dynamicMethods = from method in type.GetMethods(AllBindingFlags)
                                 let dynamicMethod = method.CreateDynamicInvoke()
                                 where dynamicMethod != null
                                 select dynamicMethod;
            return dynamicMethods.ToList();
        }

        private static Dictionary<string, DynamicField> CreateFields(Type type, MemberTypes includedMemberTypes) {
            if((includedMemberTypes & MemberTypes.Field) == 0)
                return new Dictionary<string, DynamicField>();
            var dynamicFields = from field in type.GetFields(AllBindingFlags)
                                let dynamicField = field.CreateDynamicField()
                                where dynamicField != null
                                select dynamicField;
            return dynamicFields.ToDictionary(df => df.Name);
        }

        private static Dictionary<string, DynamicProperty>
        CreateProperties(Type type, MemberTypes includedMemberTypes) {
            if((includedMemberTypes & MemberTypes.Property) == 0)
                return new Dictionary<string, DynamicProperty>();
            var dynamicProperties = from field in type.GetProperties(AllBindingFlags)
                                    let dynamicProperty = field.CreateDynamicProperty()
                                    where dynamicProperty != null
                                    select dynamicProperty;
            return dynamicProperties.ToDictionary(df => df.Name);
        }

        private static Dictionary<string, DynamicAccessor> CreateAccessors(
            IEnumerable<DynamicAccessor> dynamicFields, IEnumerable<DynamicAccessor> dynamicProperties,
            MemberTypes includedMemberTypes) {
            if(((includedMemberTypes & MemberTypes.Field) | MemberTypes.Property) == 0)
                return new Dictionary<string, DynamicAccessor>();
            var dynamicAccessors = dynamicFields.Concat(dynamicProperties);
            return dynamicAccessors.ToDictionary(df => df.Name);
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
