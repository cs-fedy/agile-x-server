using AgileX.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace AgileX.Infrastructure.Cache;

public class RedisCacheContext
{
    private readonly ConnectionMultiplexer redis;
    public IDatabase RedisDB => redis.GetDatabase();

    public RedisCacheContext(IOptions<RedisSettings> redisOptions)
    {
        RedisSettings settings = redisOptions.Value;
        ConfigurationOptions options = new() { EndPoints = { { settings.Host, settings.Port } } };
        redis = ConnectionMultiplexer.Connect(options);
    }
}
