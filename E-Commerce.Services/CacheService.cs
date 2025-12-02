using System.Text.Json;
using E_Commerce.Domain.Contracts;
using E_Commerce.Services_Abstraction;

namespace E_Commerce.Services;

public class CacheService : ICacheService
{
    private readonly ICacheRepository _cacheRepository;

    public CacheService(ICacheRepository  cacheRepository)
    {
        _cacheRepository = cacheRepository;
    }
    
    public async Task<string?> GetAsync(string cacheKey)
    {
        return await _cacheRepository.GetAsync(cacheKey);
    }

    public Task SetAsync(string cacheKey, object cacheValue, TimeSpan timeToLive)
    {
        var value = JsonSerializer.Serialize(cacheValue,new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        
        return _cacheRepository.SetAsync(cacheKey, value, timeToLive);
        
    }
}