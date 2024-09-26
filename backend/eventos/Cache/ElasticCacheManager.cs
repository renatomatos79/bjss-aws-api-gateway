using StackExchange.Redis;

namespace eventsapi;

public class ElasticCacheManager : ICacheManager 
{
    private readonly IConnectionMultiplexer connectionMultiplexer;
    private readonly IDatabase redisDb;
    public ElasticCacheManager(string connectionString)
    {
        this.connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);
        this.redisDb = connectionMultiplexer.GetDatabase();
    }
    private void AddKey(string key, string value, long expiresInSeconds)
    {
        redisDb.StringSet(key, value, TimeSpan.FromSeconds(expiresInSeconds));
    }
    private string GetKey(string key)
    {
        try 
        {
            var value = this.redisDb.StringGet(key);
            if (!value.HasValue)
            {
                return string.Empty; 
            }
            return value.IsNullOrEmpty ? string.Empty : value.ToString();
        }
        catch 
        { 
            return string.Empty; 
        }
    }
    public void Add<T>(string key, T value, long expiresInSeconds)
    {
        var item = new CacheObject<T>(value, expiresInSeconds);
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(item);
        AddKey(key, json, expiresInSeconds);
    }
    public CacheObject<T>? Get<T>(string key)
    {
        var value = GetKey(key);
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }
        return Newtonsoft.Json.JsonConvert.DeserializeObject<CacheObject<T>>(value);
    }
    
}