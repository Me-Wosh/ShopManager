using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ShopManagerBackend.Entities;
using ShopManagerBackend.Exceptions;
using ShopManagerBackend.Models;

namespace ShopManagerBackend.Services;

public interface IAuthenticationService
{
    string Login(UserLoginDto userLoginDto);
}

public class AuthenticationService : IAuthenticationService
{
    private readonly ShopManagerDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public AuthenticationService(ShopManagerDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }
    
    public string Login(UserLoginDto userLoginDto)
    {
        User? user = _dbContext.Users.SingleOrDefault(u => u.Username == userLoginDto.Username);
        
        if (user == null)
            throw new NotFoundException("User not found");

        if (!VerifyPassword(userLoginDto.Password, user.PasswordHash, user.PasswordSalt))
            throw new BadRequestException("Wrong password");

        return CreateToken(user);
    }
    
    private bool VerifyPassword(string password, byte[] hash, byte[] salt)
    {
        using HMACSHA512 hmac = new HMACSHA512(salt);
        byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        return hash.SequenceEqual(computedHash);
    }

    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, user.Role)
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("Token").Value!));
        
        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        
        JwtSecurityToken token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}