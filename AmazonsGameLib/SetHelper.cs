using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonsGameLib
{
    public static class SetHelper
    {
        public static ISet<T> ToSet<T>(this IEnumerable<T> enumerable)
        {
            HashSet<T> newSet = new HashSet<T>();
            foreach (T item in enumerable) newSet.Add(item);
            return newSet;
        }
    }
}
