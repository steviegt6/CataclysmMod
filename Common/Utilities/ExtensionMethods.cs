using System.Collections.Generic;

namespace CataclysmMod.Common.Utilities
{
    public static class ExtensionMethods
    {
        public static IDictionary<TKey, TValue> ModifyIfContains<TKey, TValue>(this IDictionary<TKey, TValue> dict,
            TKey key, TValue value)
        {
            if (dict.ContainsKey(key))
                dict[key] = value;

            return dict;
        }
    }
}