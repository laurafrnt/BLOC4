using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BCrypt.Net;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly string _hashedPassword;

    public AuthController(IConfiguration configuration)
    {
        _hashedPassword = configuration["AdminSettings:HashedPassword"];
    }

    [HttpPost("verify-password")]
    public IActionResult VerifyPassword([FromBody] string password)
    {
        if (BCrypt.Net.BCrypt.Verify(password, _hashedPassword))
        {
            return Ok(new { Message = "Mot de passe correct." });
        }
        return Unauthorized(new { Message = "Mot de passe incorrect." });
    }


}
