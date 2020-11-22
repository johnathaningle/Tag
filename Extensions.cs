using System.Collections.Generic;
using System.Linq;

namespace Tag
{
    public static class Extensions
    {
        public static IEnumerable<(int, T)> Enumerate<T>(this IEnumerable<T> source)
        {
            return source.Select((x, y) => (y, x));
        }
    }
}