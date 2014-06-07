using System.Collections.Generic;
using Nancy;

namespace Glimpse.Nancy
{
    internal static class DynamicDictionaryExtensions
    {
        public static IEnumerable<KeyValuePair<string, string>> Serialize(this DynamicDictionary dictionary)
        {
            foreach (var key in dictionary)
            {
                yield return new KeyValuePair<string, string>(key, dictionary[key]);
            }
        }

        public static IDictionary<string, string> ToStringDictionary(this DynamicDictionary dictionary)
        {
            var d = new Dictionary<string, string>();
            foreach (var key in dictionary)
            {
                d.Add(key, dictionary[key]);
            }
            return d;
        }
    }
}