using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class CollectionUtils
    {
        public static void FisherYatesShuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}