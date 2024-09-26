using System.Security.Claims;

public interface ITokenService
{
    string GenerateToken(string userId, string userName, string role);
    string GetUserName(ClaimsPrincipal user);
}