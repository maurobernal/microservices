using api.Interfaces;
using StackExchange.Redis;
using System.Text.Json;
namespace api.Context;
public class RedisContext : ICacheService
{
    private readonly IDatabaseAsync _database;
    public RedisContext( IConnectionMultiplexer connectionMultiplexer)
    {
        _database = connectionMultiplexer.GetDatabase();
    }
    public async Task<bool> DeleteDataAsync<T>(string key)
    {
       bool res = await _database.KeyExistsAsync(key);
        if (res) { 
            return await _database.KeyDeleteAsync(key);
        }
        return false;
    }

    public async Task<T> GetDataAsync<T>(string key)
    {
        var res = await _database.StringGetAsync(key);
        if (!string.IsNullOrEmpty(res))
        {
            return JsonSerializer.Deserialize<T>(res!)!;
        }
        return default;
    }

    public async Task<bool> SetDataAsync<T>(string key, T value)
    {
        var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
        TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
        var res = await _database.StringSetAsync(key, JsonSerializer.Serialize(value), expiryTime);
        return res;
    }

    public async Task<bool> SetDataAsync<T>(string key, T value, DateTimeOffset expirationTime)
    {
       TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
        var res = await _database.StringSetAsync(key, JsonSerializer.Serialize(value), expiryTime);
        return res;
    }
}
