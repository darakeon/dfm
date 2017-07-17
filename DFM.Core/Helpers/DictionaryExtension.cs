using System.Collections.Generic;
using System;
using NHibernate.Linq;

namespace DFM.Core.Helpers
{
    public static class DictionaryExtension
    {
        public static void SumItem<TKey>(this IDictionary<TKey, Double> sumList, TKey key, Double value)
        {
            if (sumList.ContainsKey(key))
                sumList[key] += value;
            else
                sumList.Add(key, value);
        }
    }
}