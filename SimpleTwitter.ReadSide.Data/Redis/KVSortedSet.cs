using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace SimpleTwitter.ReadSide.Data.Redis
{
    public class KVSortedSet: RedisDbSet
    {
        public KVSortedSet(IDatabase redisDb, string urnPrefix)
            : base(redisDb, urnPrefix)
        {
            Db = redisDb;
            AssertUrnFormat(urnPrefix, "urn:sset:");
            UrnPrefix = urnPrefix;
        }
        /// <summary>
        /// Adds all the specified members with the 0 scores (see Redis Lexicographical scores) 
        /// to the sorted set stored at key. If a specified member is already a member of the sorted set, 
        /// the score is updated and the element reinserted at the right position to ensure the correct ordering.
        /// </summary>
        /// <remarks>Remarks:http://redis.io/commands/zadd
        /// </remarks>
        /// <returns>The number of elements added to the sorted sets, not including elements already existing for which the score was updated.</returns>
        public long Add(string key, params string[] values)
        {
            var urn = GetUrn(key);
            var entries = new SortedSetEntry[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                entries[i] = new SortedSetEntry(values[i], 0);//as the score is the same for all entries the Lexicographical scoring will be applied
            }
            return Db.SortedSetAdd(urn, entries);
        }
        public bool Remove(string key, string value)
        {
            var urn = GetUrn(key);
            return Db.SortedSetRemove(urn, value);
        }

        /// <returns>True if the value was added, False if it already existed (the score is still updated)</returns>
        public bool Add(string key,double score,string value)
        {
            var urn = GetUrn(key);
            return Db.SortedSetAdd(urn, value,score);
        }


        public async Task<IEnumerable<string>> RangeByRankAsync(string key, long start = 0, long stop = -1,
            Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            var urn = GetUrn(key);
            var range = await Db.SortedSetRangeByRankAsync(urn, start, stop, order, flags);
            return range.Select(v => (string)v);
        }
    }
}