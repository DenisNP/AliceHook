using System.Collections.Generic;
using System.Linq;

namespace AliceHook
{
    public static class Utils
    {
        public static bool ContainsStartWith(this IEnumerable<string> list, string start)
        {
            return list.Any(element => element.ToLower().Trim().StartsWith(start));
        }
    }
}