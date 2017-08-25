using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.Helper
{
    public static class LinqExtensions
    {
        static Random rnd = new Random();

        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> list, int parts)
        {
            return list.Select((item, index) => new { index, item })
                       .GroupBy(x => x.index % parts)
                       .Select(x => x.Select(y => y.item));
        }

        public static List<T> GetDistinctRandom<T>(this IList<T> source, int count)
        {
            if (count > source.Count)
                throw new ArgumentOutOfRangeException();

            if (count == source.Count)
                return new List<T>(source);

            var sourceDict = source.ToIndexedDictionary();

            if (count > source.Count / 2)
            {
                while (sourceDict.Count > count)
                    sourceDict.Remove(source.GetRandomIndex());

                return sourceDict.Select(kvp => kvp.Value).ToList();
            }

            var randomDict = new Dictionary<int, T>(count);
            while (randomDict.Count < count)
            {
                int key = source.GetRandomIndex();
                if (!randomDict.ContainsKey(key))
                    randomDict.Add(key, sourceDict[key]);
            }

            return randomDict.Select(kvp => kvp.Value).ToList();
        }


        public static int GetRandomIndex<T>(this ICollection<T> source)
        {
            return rnd.Next(source.Count);
        }

        public static T GetRandom<T>(this IList<T> source)
        {
            return source[source.GetRandomIndex()];
        }

        static void AddRandomly<T>(this ICollection<T> toCol, IList<T> fromList, int count)
        {
            while (toCol.Count < count)
                toCol.Add(fromList.GetRandom());
        }

        public static Dictionary<int, T> ToIndexedDictionary<T>(this IEnumerable<T> lst)
        {
            return lst.ToIndexedDictionary(t => t);
        }

        public static Dictionary<int, T> ToIndexedDictionary<S, T>(this IEnumerable<S> lst,
                                                                   Func<S, T> valueSelector)
        {
            int index = -1;
            return lst.ToDictionary(t => ++index, valueSelector);
        }

    }
}
