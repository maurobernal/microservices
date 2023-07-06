namespace api.Interfaces;
public interface ICacheService
{
    public Task<T> GetDataAsync<T>(string key);
    public Task<bool> SetDataAsync<T>(string key, T value);
    public Task<bool> SetDataAsync<T>(string key, T value, DateTimeOffset expirationTime);
    public Task<bool> DeleteDataAsync<T>(string key);




}
