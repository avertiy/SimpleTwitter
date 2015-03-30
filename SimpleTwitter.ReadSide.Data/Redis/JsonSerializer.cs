using Newtonsoft.Json;
using StackExchange.Redis;

namespace SimpleTwitter.ReadSide.Data.Redis
{
    public class JsonSerializer : ISerializer
    {
        public JsonSerializer()
        {
            if (null == JsonConvert.DefaultSettings)
            {
                JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.All
                };
            }
        }

        public RedisValue SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public object DeserializeObject(RedisValue value)
        {
            return value.IsNull ? null : JsonConvert.DeserializeObject(value);
        }
    }
}