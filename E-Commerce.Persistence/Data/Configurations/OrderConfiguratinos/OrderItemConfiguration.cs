using E_Commerce.Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Persistence.Data.Configurations.OrderConfiguratinos;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(x => x.Price).HasPrecision(8, 2);

        builder.OwnsOne(x => x.Product, OEntity =>
        {
            OEntity.Property( p=> p.ProductName).HasMaxLength(100);
            OEntity.Property( p=> p.PictureUrl).HasMaxLength(200);
        });

    }
}