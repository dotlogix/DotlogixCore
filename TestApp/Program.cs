// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Program.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Server.Http.State;
using DotLogix.Core.Rest.Server.Routes;
using DotLogix.Core.Rest.Services.Attributes;
using DotLogix.Core.Rest.Services.Attributes.Events;
using DotLogix.Core.Rest.Services.Attributes.Http;
using DotLogix.Core.Rest.Services.Attributes.Routes;
using DotLogix.Core.Rest.Services.Base;
using DotLogix.Core.Rest.Services.Exceptions;
using DotLogix.Core.Rest.Services.Processors;
#endregion

namespace TestApp {
    internal class Program {
        private static void Main(string[] args) {
            object value=null;
            var type = typeof(object);
            Stopwatch watch = new Stopwatch();
            int iterations = 10_000;
            watch.Start();
            for(int i = 0; i < iterations; i++) {
                value = type.GetDefaultValue();
            }
            watch.Stop();
            Console.WriteLine("Own: "+watch.Elapsed);
            watch.Start();
            for (int i = 0; i < iterations; i++)
            {
                value = Activator.CreateInstance(type);
            }
            watch.Stop();
            Console.WriteLine("Theirs: " + watch.Elapsed);
            Console.WriteLine(value.ToString());
            Console.Read();
        }
    }

    internal class TestObj {
        public string Name { get; set; } = "Test";
        public int Int { get; set; } = 2;
        public bool Bool { get; set; } = true;
        public byte[] ByteArray { get; set; } = {1,2,3,4,5};
    }
}
