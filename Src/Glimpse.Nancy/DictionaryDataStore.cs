using System.Collections.Generic;
using Glimpse.Core.Extensibility;

namespace Glimpse.Nancy
{
    public class DictionaryDataStore : IDataStore
    {
        private readonly IDictionary<string, object> items;

        public DictionaryDataStore(IDictionary<string, object> items)
        {
            this.items = items;
        }

        public bool Contains(string key)
        {
            return items.ContainsKey(key);
        }

        public object Get(string key)
        {
            if (!items.ContainsKey(key)) return null;
            return items[key];
        }

        public void Set(string key, object value)
        {
            items[key] = value;
        }
    }
}