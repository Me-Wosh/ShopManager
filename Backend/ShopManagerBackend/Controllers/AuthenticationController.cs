using Microsoft.AspNetCore.Mvc;
using ShopManagerBackend.Models;
using ShopManagerBackend.Services;

namespace ShopManagerBackend.Controllers;

[ApiController]
[Route("Api/Auth/V1")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("Login")]
    public ActionResult<string> Login([FromForm]UserLoginDto userLoginDto)
    {
        return Ok(_authenticationService.Login(userLoginDto));
    }
}