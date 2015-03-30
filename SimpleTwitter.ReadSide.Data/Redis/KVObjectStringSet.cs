using System;
using StackExchange.Redis;

namespace SimpleTwitter.ReadSide.Data.Redis
{
    public class KVObjectStringSet : KVStringSet
    {
        protected readonly ISerializer Serializer;
        protected readonly IUrnResolver UrnResolver;

        public KVObjectStringSet(IDatabase redisDb, string urnPrefix, ISerializer serializer, IUrnResolver urnResolver)
            : base(redisDb, urnPrefix)
        {
            Serializer = serializer;
            UrnResolver = urnResolver;
        }
        public T StringGet<T>(string key) 
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }
            string urn = UrnResolver.ResolveUrn(typeof(T), key);
            AssertUrnFormat(urn);
            return (T)Serializer.DeserializeObject(Db.StringGet(urn));
        }

        public void StringSet<T>(string key, T value)
        {
            if (default(T).Equals(value))
            {
                throw new ArgumentNullException("value");
            }
            string urn = UrnResolver.ResolveUrn(typeof(T), key);
            AssertUrnFormat(urn);
            Db.StringSet(urn, Serializer.SerializeObject(value));
        }
    }
}