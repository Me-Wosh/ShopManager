using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShopManagerBackend.Entities.Configurations;

public class UsersConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder
            .Property(u => u.Id)
            .IsRequired();

        builder
            .Property(u => u.Username)
            .HasMaxLength(25)
            .IsRequired();

        builder
            .Property(u => u.Role)
            .HasMaxLength(20)
            .IsRequired();
        
        builder
            .Property(u => u.PasswordHash)
            .IsRequired();

        builder
            .Property(u => u.PasswordSalt)
            .IsRequired();

        CreatePasswordHash("admin", out byte[] hash, out byte[] salt);
        
        builder
            .HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Role = "Admin",
                    PasswordHash = hash,
                    PasswordSalt = salt
                }
            );
    }

    private void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
    {
        using HMACSHA512 hmac = new HMACSHA512();
        salt = hmac.Key;
        hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
}