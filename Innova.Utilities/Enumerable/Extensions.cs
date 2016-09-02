using System;
using System.Collections.Generic;

#if TEST
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Innova.Utilities.Test")]
#endif
namespace Innova.Utilities.Enumerable
{
    public static class Extensions
    {
        // # Should throw argument null exception if enumerable is null!
        // # Should return last element if any
        // # Should return default value if no elements
        // # Should return expected result without enumerating if runtime type is IList<T>
        public static T LastOrDefault<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable), "Argument is null!");
            }

            T lastElement = default(T);
            IList<T> enumerableAsList = enumerable as IList<T>;
            if (enumerableAsList != null)
            {
                if (enumerableAsList.Count > 0)
                {
                    lastElement = enumerableAsList[enumerableAsList.Count - 1];
                }
            }
            else
            {
                using (IEnumerator<T> enumerator = enumerable.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        lastElement = enumerator.Current;
                    }
                }
            }

            return lastElement;
        }
    }
}