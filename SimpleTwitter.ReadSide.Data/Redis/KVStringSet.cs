using System.Threading.Tasks;
using StackExchange.Redis;

namespace SimpleTwitter.ReadSide.Data.Redis
{
    //db0.SortedSetAdd("sortedset:followers:user1",new SortedSetEntry[]{new SortedSetEntry("Stas", 0), new SortedSetEntry("Artem", 0), new SortedSetEntry("Pete", 0)});

    public class KVStringSet : RedisDbSet
    {
        public KVStringSet(IDatabase redisDb, string urnPrefix)
            : base(redisDb, urnPrefix)
        {
        }

        public Task<RedisValue> GetAsync(string key)
        {
            return StringGetAsync(GetUrn(key));
        }

        public void Set(string key, string value)
        {
            StringSet(GetUrn(key), value);
        }
        public bool Remove(string key)
        {
            return StringRemove(GetUrn(key));
        }
        public new Task<bool> KeyExistsAsync(string key)
        {
            return base.KeyExistsAsync(GetUrn(key));
        }
    }
}