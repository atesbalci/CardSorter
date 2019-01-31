using System.Collections.Generic;
using UnityEngine;

namespace Helpers.Utilities
{
    public static class Utils
    {
        public static T RandomElement<T>(this IList<T> list)
        {
            if (list.Count <= 0) return default(T);
            return list[Random.Range(0, list.Count)];
        }

        public static T RandomElementAndRemove<T>(this IList<T> list)
        {
            if (list.Count <= 0) return default(T);
            var randomIndex = Random.Range(0, list.Count);
            var retVal = list[randomIndex];
            list.RemoveAt(randomIndex);
            return retVal;
        }

        public static List<T> CloneList<T>(this IList<T> list)
        {
            var retVal = new List<T>(list.Count);
            foreach (var ele in list)
            {
                retVal.Add(ele);
            }

            return retVal;
        }
    }
}
