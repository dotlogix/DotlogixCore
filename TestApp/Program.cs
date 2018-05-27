// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Program.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using DotLogix.Core;
using DotLogix.Core.Nodes;
#endregion

namespace TestApp {
    public class TestClass {
        public Optional<int?> OptInt { get; set; }
        public Optional<string> OptString { get; set; }
    }

    internal class Program {


        private static void Main(string[] args) {
            

            var testClass = new TestClass();
            Console.WriteLine(testClass.OptInt);

            var json = JsonNodes.ToJson(testClass);
            Console.WriteLine(json);
            var instance = JsonNodes.FromJson<TestClass>(json);


            testClass.OptInt = 1;
            testClass.OptString = "test";
            json = JsonNodes.ToJson(testClass);
            Console.WriteLine(json);
            instance = JsonNodes.FromJson<TestClass>(json);

            testClass.OptInt = null;
            testClass.OptString = null;
            json = JsonNodes.ToJson(testClass);
            Console.WriteLine(json);
            instance = JsonNodes.FromJson<TestClass>(json);


            Console.Read();
        }
    }
}
