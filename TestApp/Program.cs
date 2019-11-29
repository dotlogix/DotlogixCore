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
using System.Linq;
using System.Threading.Tasks;
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
            [NodeProperty(EmitMode = EmitMode.IgnoreDefault)]
            public int DefaultInt { get; set; }

            [NodeProperty(EmitMode = EmitMode.IgnoreDefault)]
            public string DefaultString { get; set; }
        }

        private static async Task Main(string[] args) {
            var person = new Person {FirstName = "Alex", LastName = "Schill", Enumerable = new []{0, 1,2,0,3,4}};


            var json = JsonNodes.ToJson(person);
            Console.WriteLine(json);

            var deserializedPerson = JsonNodes.FromJson<Person>(json);

            Console.Read();
        }

    }
}
