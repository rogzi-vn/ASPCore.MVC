using System.Collections.Generic;

namespace ASPCoreMVC.Common
{
    public class BaseMap<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public BaseMap(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
    public class BaseMap : BaseMap<string, string>
    {
        public BaseMap(string key, string value) : base(key, value)
        {
        }
    }
}
