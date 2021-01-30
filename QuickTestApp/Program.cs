using System;
using DotLogix.Core;
using DotLogix.Core.Reflection.Dynamics;
using Newtonsoft.Json;

namespace ConsoleApp1 {
    public class Program {
        public static void Main(string[] args) {
            var ctor = typeof(Optional<>).MakeGenericType(typeof(TestEnum)).CreateDynamicType().GetConstructor(typeof(TestEnum));

            var deserialize = JsonConvert.DeserializeObject("0", typeof(TestEnum));
            
            var value = ctor.Invoke(deserialize);
            
            Console.WriteLine(value);
            
            Console.ReadLine();
        }
    }
}
