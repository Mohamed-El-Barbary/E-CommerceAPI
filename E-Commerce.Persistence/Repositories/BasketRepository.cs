using System.Text.Json;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.BasketModule;
using StackExchange.Redis;

namespace E_Commerce.Persistence.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDatabase _database;

    public BasketRepository(IConnectionMultiplexer connection)
    {
        _database = connection.GetDatabase();
    }

    public async Task<CustomerBasket?> GetBasketAsync(string basketId)
    {
        var basket = await _database.StringGetAsync(basketId);

        if (basket.IsNullOrEmpty)
            return null;
        else
            return JsonSerializer.Deserialize<CustomerBasket>(basket);
    }

    public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan timeToLive = default)
    {
        var jsonBasket = JsonSerializer.Serialize(basket);
        var isCreatedOrUpdayed = await _database.StringSetAsync(basket.Id, jsonBasket,
            (timeToLive == default) ? TimeSpan.FromDays(7) : timeToLive);

        if (isCreatedOrUpdayed)
        {
            return await GetBasketAsync(basket.Id);
        }
        else
        {
            return null;
        }
    }

    public Task<bool> DeleteBasketAsync(string basketId) => _database.KeyDeleteAsync(basketId);
}