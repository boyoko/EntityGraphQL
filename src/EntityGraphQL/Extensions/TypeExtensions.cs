using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace EntityGraphQL.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsEnumerable(this Type source)
        {
            if (source == typeof(string) || source == typeof(byte[]))
                return false;

            var isEnumerable = source.GetTypeInfo().IsGenericType && source.GetGenericTypeDefinition() == typeof(IEnumerable<>);
            if (!isEnumerable)
            {
                foreach (var intType in source.GetInterfaces())
                {
                    isEnumerable = intType.IsEnumerable();
                    if (isEnumerable)
                        break;
                }
            }
            return isEnumerable;
        }
        public static Type GetEnumerableType(this Type type)
        {
            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return type.GetGenericArguments()[0];
            foreach (var intType in type.GetInterfaces())
            {
                if (intType.IsEnumerable())
                {
                    return intType.GetGenericArguments()[0];
                }
                var deepIntType = intType.GetEnumerableType();
                if (deepIntType != null)
                    return deepIntType.GetGenericArguments()[0];
            }
            return null;
        }

        public static bool IsNullableType(this Type t)
        {
            return t.GetTypeInfo().IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}
