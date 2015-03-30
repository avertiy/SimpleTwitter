using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace SimpleTwitter.ReadSide.Data.Redis
{
    //public interface IRedisStorage
    //{
    //    T StringGet<T>(string key) where T : IKVEntity;
    //    void StringSet<T>(T value) where T : class,IKVEntity;
    //    void StringSet(string urn, string value);
    //    string StringGet(string urn);
    //    bool StringRemove(string urn);
    //    void AssertUrnFormat(string urn, string prefixprefix = "urn:");

    //    IEnumerable<string> SortedSetRangeByRank(string urn, long start = 0, long stop = -1,
    //        Order order = Order.Ascending, CommandFlags flags = CommandFlags.None);

    //    long SortedSetAdd(string urn, params string[] values);
    //}

    public abstract class RedisDbSet
    {
        protected IDatabase Db { get;  set; }
        public string UrnPrefix { get; set; }
        protected RedisDbSet(IDatabase redisDb, string urnPrefix)
        {
            Db = redisDb;
            AssertUrnFormat(urnPrefix);
            UrnPrefix = urnPrefix;
        }
        protected void AssertUrnFormat(string urn, string prefix = "urn:")
        {
            if (string.IsNullOrEmpty(urn))
            {
                throw new ArgumentNullException("urn");
            }
            //todo replace with Regex expr.
            if (!urn.StartsWith(prefix))
            {
                throw new FormatException(string.Format("urn '{0}' should start with '{1}' prefix", urn, prefix));
            }
        }

        protected void StringSet(string urn, string value)
        {
            AssertUrnFormat(urn);
            Db.StringSet(urn, value);
        }
        protected Task<RedisValue> StringGetAsync(string urn)
        {
            return Db.StringGetAsync(urn);
        }
        protected bool StringRemove(string urn)
        {
            return Db.KeyDelete(urn);
        }
        protected Task<bool> KeyExistsAsync(string urn)
        {
            return Db.KeyExistsAsync(urn);
        }

        public string GetUrn(string key)
        {
            return UrnPrefix + ":" + key;
        }

        protected long SortedSetAdd(string urn, params string[] values)
        {
            AssertUrnFormat(urn);
            SortedSetEntry[] entries = new SortedSetEntry[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                entries[i] = new SortedSetEntry(values[i], 0);//use Lexicographical scores
            }
            return Db.SortedSetAdd(urn, entries);
        }
    }

    
}