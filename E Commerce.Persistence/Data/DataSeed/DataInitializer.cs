using System.Text.Json;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Persistence.Data.DataSeed;

public class DataInitializer : IDataInitializer
{
    private readonly StoreDbContext _dbContext;

    public DataInitializer(StoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task InitializeAsync()
    {
        try
        {
            var hasProduct = await _dbContext.Products.AnyAsync();
            var hasProductBrands = await _dbContext.ProductBrands.AnyAsync();
            var hasProductTypes = await _dbContext.ProductTypes.AnyAsync();

            if (hasProductTypes && hasProductBrands && hasProduct) return;

            if (!hasProductBrands)
                SeedDataFromJson<ProductBrand, int>("brands.json", _dbContext.ProductBrands);

            if (!hasProductTypes)
                SeedDataFromJson<ProductType, int>("types.json", _dbContext.ProductTypes);

            await _dbContext.SaveChangesAsync();

            if (!hasProduct)
                SeedDataFromJson<Product, int>("products.json", _dbContext.Products);

            await _dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task SeedDataFromJson<T, TEntity>(string fileName, DbSet<T> dbSet) where T : BaseEntity<TEntity>
    {
        // D:\Route Bootcamp Back_End\API\E-CommerceAPI\E-CommerceSolution\E Commerce.Persistence\Data\DataSeed\JSONFiles\brands.json
        var filePath = @"..\E Commerce.Persistence\Data\DataSeed\JSONFiles\brands.json" + fileName;

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File {fileName} is not Exist");

        try
        {
            using var dataStream = File.OpenRead(filePath);
            var data = await JsonSerializer.DeserializeAsync<T>(dataStream, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            if (data is not null)
                await dbSet.AddRangeAsync(data);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error While Reading JSON File {e}");
            return;
        }
    }
}