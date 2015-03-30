using StackExchange.Redis;

namespace SimpleTwitter.ReadSide.Data.Redis
{
    public interface ISerializer
    {
        RedisValue SerializeObject(object obj);
        object DeserializeObject(RedisValue value);
    }
}