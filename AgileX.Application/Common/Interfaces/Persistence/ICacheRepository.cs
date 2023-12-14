namespace AgileX.Application.Common.Interfaces.Persistence;

public interface ICacheRepository
{
    Task Cache<TValue>(string key, TValue value, DateTime expiresIn);
    Task Remove(string key);
    Task<object?> Fetch<TValue>(string key);
    Task<bool> Exists(string key);
}
