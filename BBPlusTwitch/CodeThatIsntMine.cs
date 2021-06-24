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

        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> items, Func<T, TKey> property) //thank you satpal: https://stackoverflow.com/users/1668533/satpal
        {
            return items.GroupBy(property).Select(x => x.First());
        }

    }
}
