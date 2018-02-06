using System;
using System.Text;

namespace DotLogix.UI.Animations {
    public static class TypeExtensions {
        public static string ReadableName(this Type type) {
            var sb = new StringBuilder();
            BuildName(sb, type);
            return sb.ToString();
        }

        private static void BuildName(StringBuilder sb, Type type) {
            if(!type.IsGenericType) {
                sb.Append(type.Name);
            } else {
                var name = type.Name.Split("`".ToCharArray())[0];
                sb.Append(name);
                sb.Append("<");
                foreach(var param in type.GetGenericArguments()) {
                    BuildName(sb, param);
                    sb.Append(",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(">");
            }
        }
    }
}