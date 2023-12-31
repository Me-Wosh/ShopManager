using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShopManagerBackend.Entities.Configurations;

public class ProductsConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder
            .Property(p => p.Id)
            .IsRequired();

        builder
            .Property(p => p.Sku)
            .IsRequired()
            .HasMaxLength(50);

        builder
            .Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder
            .Property(p => p.Quantity)
            .IsRequired()
            .HasDefaultValue(0);

        builder
            .Property(p => p.Price)
            .IsRequired()
            // up to 999 billions 99 divisional currency
            .HasPrecision(14, 2);

        builder
            .HasData(
                new Product
                {
                    Id = 1,
                    Sku = "HX421RO",
                    Name = "Hat",
                    Quantity = 40,
                    Price = 16.90m,
                    ImagePath = "Assets/Hat.svg"
                },
                
                new Product
                {
                    Id = 2,
                    Sku = "MN632FD",
                    Name = "Hoodie",
                    Quantity = 80,
                    Price = 35.90m,
                    ImagePath = "Assets/Hoodie.svg"
                },
                
                new Product
                {
                    Id = 3,
                    Sku = "DX218ED",
                    Name = "Jacket",
                    Quantity = 50,
                    Price = 49.90m,
                    ImagePath = "Assets/Jacket.svg"
                },
                
                new Product
                {
                    Id = 4,
                    Sku = "SI972YV",
                    Name = "Pants",
                    Quantity = 70,
                    Price = 27.90m,
                    ImagePath = "Assets/Pants.svg"
                },
                
                new Product
                {
                    Id = 5,
                    Sku = "FL560RX",
                    Name = "Shirt",
                    Quantity = 35,
                    Price = 19.90m,
                    ImagePath = "Assets/Shirt.svg"
                },
                
                new Product
                {
                    Id = 6,
                    Sku = "QJ736BG",
                    Name = "Shoes",
                    Quantity = 90,
                    Price = 59.90m,
                    ImagePath = "Assets/Shoes.svg"
                },
                
                new Product
                {
                    Id = 7,
                    Sku = "CM214JK",
                    Name = "Sweater",
                    Quantity = 25,
                    Price = 30.90m,
                    ImagePath = "Assets/Sweater.svg"
                }
            );
    }
}