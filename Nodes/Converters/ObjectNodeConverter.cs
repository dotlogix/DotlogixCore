// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ObjectNodeConverter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Linq;
using System.Reflection;
using DotLogix.Core.Nodes.Io;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Converters {
    public class ObjectNodeConverter : NodeConverter {
        private readonly DynamicAccessor[] _accessorsToRead;
        private readonly DynamicAccessor[] _accessorsToWrite;
        private readonly DynamicCtor _ctor;
        private readonly bool _isDefaultCtor;
        public DynamicType DynamicType { get; }

        public ObjectNodeConverter(DataType dataType, AccessorTypes accessorTypes,
                                   bool useReadonly) : base(dataType) {
            var accessorMemberTypes = GetAccessorMemberTypes(accessorTypes);
            var dynamicType = Type.CreateDynamicType(MemberTypes.Constructor | accessorMemberTypes);

            DynamicType = dynamicType;
            var readAccessModes = useReadonly ? ValueAccessModes.Read : ValueAccessModes.ReadWrite;
            _accessorsToRead = dynamicType.GetAccessors(accessorTypes, readAccessModes).ToArray();
            _accessorsToWrite = dynamicType.GetAccessors(accessorTypes).ToArray();

            _isDefaultCtor = dynamicType.HasDefaultConstructor;
            if(_isDefaultCtor) {
                _ctor = dynamicType.GetDefaultConstructor();
                return;
            }

            foreach(var ctor in dynamicType.Constructors) {
                if(CanConstructWith(ctor, _accessorsToRead, out var neededAccessors)) {
                    _ctor = ctor;
                    _accessorsToWrite = _accessorsToWrite.Except(neededAccessors).ToArray();
                    return;
                }
            }

            if(_ctor == null) {
                throw new
                InvalidOperationException($"Can not find any usable constructor for type {dynamicType.Type.FullName}");
            }
        }

        private static MemberTypes GetAccessorMemberTypes(AccessorTypes accessorTypes) {
            MemberTypes memberTypes;
            switch(accessorTypes) {
                case AccessorTypes.None:
                    memberTypes = 0;
                    break;
                case AccessorTypes.Property:
                    memberTypes = MemberTypes.Property;
                    break;
                case AccessorTypes.Field:
                    memberTypes = MemberTypes.Field;
                    break;
                case AccessorTypes.Any:
                    memberTypes = MemberTypes.Property | MemberTypes.Field;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(accessorTypes), accessorTypes, null);
            }
            return memberTypes;
        }

        public override void Write(object instance, string rootName, INodeWriter writer) {
            writer.BeginMap(rootName);
            foreach(var accessor in _accessorsToRead)
                Nodes.WriteToInternal(accessor.Name, accessor.GetValue(instance), accessor.ValueType, writer);
            writer.EndMap();
        }

        public override object ConvertToObject(Node node) {
            if(!(node is NodeMap nodeMap))
                throw new ArgumentException("Node is not a NodeMap");

            object instance;
            if(_isDefaultCtor)
                instance = _ctor.Invoke();
            else if(TryConstructWith(_ctor, nodeMap, out instance))
                throw new InvalidOperationException("Object can not be constructed with the given nodes");

            foreach(var accessor in _accessorsToWrite) {
                var accessorNode = nodeMap.GetChild(accessor.Name);
                if(accessorNode == null)
                    throw new InvalidOperationException("Object can not be constructed with the given nodes");
                var accessorValue = Nodes.ToObject(accessorNode, accessor.ValueType);
                accessor.SetValue(instance, accessorValue);
            }
            return instance;
        }

        private bool CanConstructWith(DynamicCtor ctor, DynamicAccessor[] accessorsToRead,
                                      out DynamicAccessor[] neededAccessors) {
            neededAccessors = null;
            var parameters = ctor.Parameters;
            var parameterCount = parameters.Length;
            var accessors = new DynamicAccessor[parameterCount];
            for(var i = 0; i < parameterCount; i++) {
                var parameter = parameters[i];
                var accessor =
                accessorsToRead.FirstOrDefault(a => a.Name.Equals(parameter.Name, StringComparison.OrdinalIgnoreCase));
                if(accessor == null)
                    return false;
                accessors[i] = accessor;
            }
            neededAccessors = accessors;
            return true;
        }

        private bool TryConstructWith(DynamicCtor ctor, NodeMap nodeMap, out object instance) {
            instance = null;

            var parameters = ctor.Parameters;
            var parameterCount = parameters.Length;
            var parametersForCtor = new object[parameterCount];
            for(var i = 0; i < parameterCount; i++) {
                var parameter = parameters[i];
                var parameterNode = nodeMap.GetChild(parameter.Name);
                if(parameterNode == null)
                    return false;

                parametersForCtor[i] = Nodes.ToObject(parameterNode, parameter.ParameterType);
            }

            instance = ctor.Invoke(parametersForCtor);
            return true;
        }
    }
}
