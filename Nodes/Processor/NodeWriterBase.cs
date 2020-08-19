// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeWriterBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Threading.Tasks;
#endregion

namespace DotLogix.Core.Nodes.Processor {
    public abstract class NodeWriterBase : IAsyncNodeWriter {
        protected readonly NodeContainerStack ContainerStack = new NodeContainerStack();
        protected NodeWriterBase(ConverterSettings converterSettings = null) {  
            ConverterSettings = converterSettings ?? ConverterSettings.Default;
        }

        protected string CurrentName { get; set; }
        protected ConverterSettings ConverterSettings { get; }

        #region Async

        public Task WriteNameAsync(string name)
        {
            CurrentName = name;
            return Task.CompletedTask;
        }

        public abstract Task WriteBeginMapAsync();
        public abstract Task WriteEndMapAsync();

        public abstract Task WriteBeginListAsync();
        public abstract Task WriteEndListAsync();

        public abstract Task WriteValueAsync(object value);

        public async Task WriteOperationAsync(NodeOperation operation)
        {
            var name = operation.Name ?? CurrentName;
            CurrentName = null;
            if (string.IsNullOrEmpty(name) == false)
                await WriteNameAsync(name).ConfigureAwait(false);

            switch(operation.Type) {
                case NodeOperationTypes.BeginMap:
                    await WriteBeginMapAsync().ConfigureAwait(false);
                    break;
                case NodeOperationTypes.EndMap:
                    await WriteEndMapAsync().ConfigureAwait(false);
                    break;
                case NodeOperationTypes.BeginList:
                    await WriteBeginListAsync().ConfigureAwait(false);
                    break;
                case NodeOperationTypes.EndList:
                    await WriteEndListAsync().ConfigureAwait(false);
                    break;
                case NodeOperationTypes.Value:
                    await WriteValueAsync(operation.Value).ConfigureAwait(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
    }
}
