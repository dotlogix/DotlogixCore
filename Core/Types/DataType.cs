// ==================================================
// Copyright 2016(C) , DotLogix
// File:  DataType.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  05.10.2017
// LastEdited:  01.11.2017
// ==================================================

#region
using System;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Types {
    public class DataType {
        public static DataType EmptyType { get; } = new DataType(DataTypeFlags.None, null);

        public DataTypeFlags Flags { get; }
        public Type Type { get; }
        public Type UnderlayingType { get; }
        public Type ElementType { get; }

        public DataType UnderlayingDataType => UnderlayingType?.ToDataType();

        public DataType ElementDataType => ElementType?.ToDataType();

        public DataType(DataTypeFlags flags, Type type, Type underlayingType = null, Type elementType=null) {
            Flags = flags;
            Type = type;
            UnderlayingType = underlayingType;
            ElementType = elementType;
        }

        protected bool Equals(DataType other) {
            return Type == other.Type;
        }

        public override bool Equals(object obj) {
            if(ReferenceEquals(null, obj))
                return false;
            if(ReferenceEquals(this, obj))
                return true;
            if(obj.GetType() != GetType())
                return false;
            return Equals((DataType)obj);
        }

        public override int GetHashCode() {
            return Type != null ? Type.GetHashCode() : 0;
        }
    }
}
