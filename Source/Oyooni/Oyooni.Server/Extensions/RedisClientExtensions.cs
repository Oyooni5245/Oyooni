using ServiceStack.Redis;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Extensions
{
    public static class RedisClientExtensions
    {
        public static async Task<T> GetAndSerializeValueAsync<T>(this IRedisClientAsync redisClient, string key, CancellationToken token = default) where T: class, new()
        {
            return (await redisClient.GetValueAsync(key, token))?.ToObject<T>();
        }
    }
}
