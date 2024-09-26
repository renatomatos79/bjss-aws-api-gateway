using Microsoft.AspNetCore.Mvc;

namespace eventsapi.Controllers;

[ApiController]
[Route("api")]
public class LoginController : ControllerBase
{
    private readonly ITokenService tokenService;
    public LoginController(ITokenService tokenService)
    {
        this.tokenService = tokenService;
    }

    [HttpPost("token")]
    public string Token([FromBody] LoginRequest request)
    {
        if (request == null || !request.isValid())
        {
            throw new ArgumentNullException("Request is not valid.");
        }
        
        var user = GlobalConstants.Users.FirstOrDefault(u => u.Username == request.userName && u.Password == request.password);
        if (user == null)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        return tokenService.GenerateToken(user.Id, user.Name, user.Role);
    }
}
