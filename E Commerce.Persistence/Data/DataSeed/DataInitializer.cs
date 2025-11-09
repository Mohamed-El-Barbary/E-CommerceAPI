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

    public void Initialize()
    {
        try
        {
            var hasProduct = _dbContext.Products.Any();
            var hasProductBrands = _dbContext.ProductBrands.Any();
            var hasProductTypes = _dbContext.ProductTypes.Any();

            if (hasProductTypes && hasProductBrands && hasProduct) return;

            if (!hasProductBrands)
                SeedDataFromJson<ProductBrand, int>("brands.json", _dbContext.ProductBrands);

            if (!hasProductTypes)
                SeedDataFromJson<ProductType, int>("types.json", _dbContext.ProductTypes);
            
            _dbContext.SaveChanges();
            if (!hasProduct)
                SeedDataFromJson<Product, int>("products.json", _dbContext.Products);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void SeedDataFromJson<T, TEntity>(string fileName, DbSet<T> dbSet) where T : BaseEntity<TEntity>
    {
        // D:\Route Bootcamp Back_End\API\E-CommerceAPI\E-CommerceSolution\E Commerce.Persistence\Data\DataSeed\JSONFiles\brands.json
        var filePath = @"..\E Commerce.Persistence\Data\DataSeed\JSONFiles\brands.json" + fileName;

        if (!File.Exists(filePath)) 
            throw new FileNotFoundException($"File {fileName} is not Exist");
        
        try
        {
            using var dataStream = File.OpenRead(filePath);
            var data = JsonSerializer.Deserialize<T>(dataStream, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
            
            if (data is not null)
                dbSet.AddRange(data);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error While Reading JSON File {e}");
            return;
        }
    }
}