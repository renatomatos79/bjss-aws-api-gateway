namespace eventsapi;

public interface ICacheManager
{
    void Add<T>(string key, T value, long expiresInSeconds);
    CacheObject<T>? Get<T>(string key);
}