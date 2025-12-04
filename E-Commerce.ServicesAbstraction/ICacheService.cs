namespace E_Commerce.Services_Abstraction;

public interface ICacheService
{
    
    Task<string?> GetAsync(string cacheKey);
    
    Task SetAsync(string cacheKey, object cacheValue, TimeSpan timeToLive);

}