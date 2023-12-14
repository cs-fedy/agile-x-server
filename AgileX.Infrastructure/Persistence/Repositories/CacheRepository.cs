using AgileX.Application.Common.Interfaces.Persistence;
using StackExchange.Redis;

namespace AgileX.Infrastructure.Persistence.Repositories;

public class CacheRepository : ICacheRepository
{
    private readonly IDatabase _db;

    public CacheRepository(IDatabase db) => _db = db;

    public async Task Cache<TValue>(string key, TValue value, DateTime expiresIn)
    {
        var serializedValue = Serialize<TValue>(value);
        await _db.StringSetAsync(key, serializedValue, expiresIn.TimeOfDay);
    }

    public async Task Remove(string key)
    {
        _db.KeyDelete(key);
        await Task.CompletedTask;
    }

    public async Task<object?> Fetch<TValue>(string key)
    {
        var cachedData = await _db.StringGetAsync(key);
        if (cachedData.IsNull)
            return null;

        TValue deserializedValue = Deserialize<TValue>(cachedData!);
        return Task.FromResult(deserializedValue);
    }

    public async Task<bool> Exists(string key)
    {
        var cachedData = await _db.StringGetAsync(key);
        return cachedData.HasValue;
    }

    private string? Serialize<TValue>(object? obj) => obj?.ToString();

    private TValue Deserialize<TValue>(string serializedValue) =>
        (TValue)Convert.ChangeType(serializedValue, typeof(TValue));
}
