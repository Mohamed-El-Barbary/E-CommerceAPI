using E_Commerce.Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Persistence.Data.Configurations.OrderConfiguratinos;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(o => o.SubTotal)
            .HasPrecision(8, 2);

        builder.OwnsOne(o => o.Address, OEntity =>
        {
            OEntity.Property(o => o.FirstName).HasMaxLength(50);
            OEntity.Property(o => o.LastName).HasMaxLength(50);
            OEntity.Property(o => o.Street).HasMaxLength(50);
            OEntity.Property(o => o.City).HasMaxLength(50);
            OEntity.Property(o => o.Country).HasMaxLength(50);
        });
    }
}