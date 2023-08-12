using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShopManagerBackend.Controllers;
using ShopManagerBackend.Entities;
using ShopManagerBackend.Exceptions;
using ShopManagerBackend.Models;
using ShopManagerBackend.Services;

namespace IntegrationTests;

public class AuthenticationControllerTests
{
    private readonly AuthenticationController _controller;

    public AuthenticationControllerTests()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, false)
            .AddEnvironmentVariables()
            .Build();
        
        CreatePasswordHash("admin", out byte[] hash, out byte[] salt);
        
        var adminUser = new User
        {
            Username = "admin",
            Role = "Admin",
            PasswordHash = hash,
            PasswordSalt = salt
        };

        var dbContext = CreateDbContext(new[] { adminUser });
        var service = new AuthenticationService(dbContext, configuration);
        _controller = new AuthenticationController(service);
    }
    
    [Fact]
    public void Login_GivenCorrectUsernameAndPassword_ReturnsJWT()
    {
        var userLoginDto = new UserLoginDto
        {
            Username = "admin",
            Password = "admin"
        };

        var result = (ObjectResult)_controller.Login(userLoginDto).Result!;
        var periods = ((string)result.Value!).Split('.');
        
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(result.Value);
        Assert.IsType<string>(result.Value);
        Assert.Equal(3, periods.Length);
    }

    [Fact]
    public void Login_GivenIncorrectUsername_Throws404NotFound()
    {
        var userLoginDto = new UserLoginDto
        {
            Username = "john",
            Password = "wick"
        };

        var exception = Record.Exception(() => _controller.Login(userLoginDto));
        Assert.NotNull(exception);
        Assert.IsType<NotFoundException>(exception);
        Assert.Equal("User not found", exception.Message);
    }

    [Fact]
    public void Login_GivenIncorrectPassword_Throws400BadRequest()
    {
        var userLoginDto = new UserLoginDto
        {
            Username = "admin",
            Password = "wick"
        };

        var exception = Record.Exception(() => _controller.Login(userLoginDto));
        Assert.NotNull(exception);
        Assert.IsType<BadRequestException>(exception);
        Assert.Equal("Wrong password", exception.Message);
    }
    
    private ShopManagerDbContext CreateDbContext<T>(IEnumerable<T> data) where T : class
    {
        var dbContext = new ShopManagerDbContext(new DbContextOptionsBuilder<ShopManagerDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        
        dbContext.AddRange(data);
        dbContext.SaveChanges();

        return dbContext;
    }
        
    private void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
    {
        using HMACSHA512 hmac = new HMACSHA512();
        salt = hmac.Key;
        hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
}