// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Program.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Benchmarks.Serializers;
using DotLogix.Core;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Nodes;
using DotLogix.Core.Nodes.Processor;
#endregion

namespace TestApp {
    //public class DynamicConvertable : DynamicObject {
    //    public object Value { get; }

    //    public DynamicConvertable(object value) {
    //        Value = value;
    //    }

    //    public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result) {
    //        if(Value == null) {
    //            result = null;
    //            return false;
    //        }

    //        var type = Value.GetType();
    //        var method = type.GetMethod(binder.Name, args.ToTypeArray());
    //        if(method == null) {
    //            result = null;
    //            return false;
    //        }

    //        result = method.CreateDynamicInvoke().Invoke(Value, args);
    //        return true;
    //    }

    //    public override bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object result) {
    //        var binary = Expression.MakeBinary(binder.Operation, Expression.Constant(Value), Expression.Constant(arg));
    //        var lambda = Expression.Lambda(binary);

    //        try {
    //            result = lambda.Compile().DynamicInvoke();
    //            return true;
    //        } catch {
    //            result = null;
    //            return false;
    //        }
    //    }

    //    public override bool TryUnaryOperation(UnaryOperationBinder binder, out object result) {
    //        var binary = Expression.MakeUnary(binder.Operation, Expression.Constant(Value), binder.ReturnType);
    //        var lambda = Expression.Lambda(binary);

    //        try
    //        {
    //            result = lambda.Compile().DynamicInvoke();
    //            return true;
    //        }
    //        catch
    //        {
    //            result = null;
    //            return false;
    //        }
    //    }

    //    public override bool TryConvert(ConvertBinder binder, out object result) {
    //        return Value.TryConvertTo(binder.ReturnType, out result);
    //    }

    //    public override string ToString() {
    //        return Value?.ToString();
    //    }
    //}

    

    internal class Program {
        public class Person {
            [NodeProperty(Name = "test")]
            public string FirstName { get; set; }



            [NodeProperty(NamingStrategy = typeof(SnakeCaseNamingStrategy))]
            public string LastName { get; set; }



            [NodeChild(EmitMode = EmitMode.IgnoreDefault)]
            public IEnumerable<int> Enumerable { get; set; }


            [NodeProperty(EmitMode = EmitMode.IgnoreDefault, NumberFormat = "X8")]
            public int DefaultInt { get; set; } = 255;



            [NodeProperty(EmitMode = EmitMode.IgnoreDefault)]
            public string DefaultString { get; set; }


            [NodeProperty(EmitMode = EmitMode.IgnoreDefault, NumberFormat = "P")]
            public double PerctDouble { get; set; } = 0.85;



            [NodeProperty(EmitMode = EmitMode.IgnoreNull)]
            public Optional<string> DefaultOptionalString { get; set; }
        }

        public class PersonPlain {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public IEnumerable<int> Enumerable { get; set; }
            public int DefaultInt { get; set; }
            public string DefaultString { get; set; }
            public Optional<string> DefaultOptionalString { get; set; }
        }

        private static async Task Main(string[] args) {
            Log.LogLevel = LogLevels.Trace;
            Log.AttachLoggers(new ConsoleLogger(120, 100));
            Log.Initialize();


            Log.Info("Test");
            try {
                throw new Exception("error 1234");
            } catch(Exception e) {
                Log.Error(e);
            }

            Console.ReadLine();


            //var config = new DebugBuildConfig();
            //BenchmarkRunner.Run<SerializeToString<Models.SmallClass>>();
            //BenchmarkRunner.Run<SerializeToString<Models.ThousandSmallClassDictionary>>();
            //BenchmarkRunner.Run<DeserializeFromString<Models.ThousandSmallClassDictionary>>();

            //var model = new Models.ThousandSmallClassDictionary();

            //Benchmark(model, 1);


            //Benchmark(person, 100_000);

            Console.ReadLine();
        }

        private static void Benchmark<T>(T person, int iterations) {
            // Warmup
            string json = JsonNodes.ToJson(person);
            T result = JsonNodes.FromJson<T>(json);

            Console.WriteLine($"Benchmark: {typeof(T).Name}");

            var watch = new Stopwatch();

            watch.Start();
            for (int i = 0; i < iterations; i++)
                json = JsonNodes.ToJson(person);
            watch.Stop();
            Console.WriteLine($"Serialization: {watch.Elapsed.TotalMilliseconds}ms");

            watch.Restart();
            for (int i = 0; i < iterations; i++)
                result = JsonNodes.FromJson<T>(json);
            watch.Stop();
            Console.WriteLine($"Deserialization: {watch.Elapsed.TotalMilliseconds}ms");

            Console.Write(json);
            Console.Write(result);
        }
    }
}
