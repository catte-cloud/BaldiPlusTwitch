using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;

namespace StolenYetHelpfulCode
{
    public static class CodeThatIsntMine
    {
        public static object InvokeMethod<T>(this T obj, string methodName, params object[] args) //thank you owen james: https://stackoverflow.com/users/2736798/owen-james
        {
            var type = typeof(T);
            var method = type.GetTypeInfo().GetDeclaredMethod(methodName);
            return method.Invoke(obj, args);
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) //thanks to fasguy(fasfuck) for this code
        {
            HashSet<TKey> knownKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static object GrabPrivateVariable<T>(this T obj, string varname) //partially copied from here with some modifications: https://www.c-sharpcorner.com/blogs/setting-and-getting-private-variable-of-a-class-without-properties
        {
            FieldInfo type = obj.GetType().GetField(varname, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return type.GetValue(obj);
        }

        public static void SetPrivateVariable<T>(this T obj, string varname, object set) //partially copied from here with some modifications: https://www.c-sharpcorner.com/blogs/setting-and-getting-private-variable-of-a-class-without-properties
        {
            FieldInfo type = obj.GetType().GetField(varname, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            type.SetValue(obj,set);
        }

    }
}
