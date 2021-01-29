// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeWriter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  16.07.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Nodes.Schema;
#endregion

namespace DotLogix.Core.Nodes.Formats.Nodes {
    public class VoidWriter : NodeWriterBase {
        public VoidWriter(IReadOnlyConverterSettings converterSettings = null) : base(converterSettings) { }
        private bool IsStarted { get; set; }
        public bool IsComplete => IsStarted && ContainerStack.Count == 0;

        #region 

        public override void WriteBeginMap() {
            CheckName(CurrentName);
            CurrentName = null;
            ContainerStack.Push(NodeContainerType.Map);
            IsStarted = true;
        }

        public override void WriteEndMap() {
            ContainerStack.PopExpected(NodeContainerType.Map);
        }

        public override void WriteBeginList() {
            CheckName(CurrentName);
            CurrentName = null;
            ContainerStack.Push(NodeContainerType.List);
            IsStarted = true;
        }

        public override void WriteEndList() {
            ContainerStack.PopExpected(NodeContainerType.List);
        }

        public override void WriteValue(object value) {
            CheckName(CurrentName);
            CurrentName = null;
            IsStarted = true;
        }

        #endregion

        private void CheckName(string name) {
            switch(ContainerStack.Current) {
                case NodeContainerType.Map:
                    if(name == null)
                        throw new ArgumentNullException(nameof(name), $"Name can not be null in the current container {ContainerStack.Current}");
                    break;
                case NodeContainerType.List:
                    if(name != null)
                        throw new ArgumentException(nameof(name), $"Name can not have a value in the current container {ContainerStack.Current}");
                    break;
                case NodeContainerType.None:
                    if(IsStarted)
                        throw new InvalidOperationException($"Name can not have a value in the current container {ContainerStack.Current}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
