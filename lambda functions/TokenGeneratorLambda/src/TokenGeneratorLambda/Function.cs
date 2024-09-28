using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Amazon.Lambda.Core;
using Microsoft.IdentityModel.Tokens;

// Lambda function handler class
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace TokenGeneratorLambda
{
    // Lambda function handler
    public class Function
    {
        public string FunctionHandler(TokenRequest request, ILambdaContext context)
        {
            var tcr = new TokenCreateRequest
            {
                Username = request.Username,
                Password = request.Password,
                UserId = "1",
                RoleName = "Admin",
                SecretKey = "33a19758-19c0-4be4-9582-f010eb7928f4",
                Issuer = "https://bjss-aws.pt",
                Audience = "https://bjss-aws.pt"
            };
            return GenerateToken(tcr);
        }

        public string GenerateToken(TokenCreateRequest tokenRequest)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenRequest.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Sid, tokenRequest.UserId),
            new Claim(ClaimTypes.NameIdentifier, tokenRequest.Username),
            new Claim(ClaimTypes.Role, tokenRequest.RoleName)
        };

            var token = new JwtSecurityToken(
                issuer: tokenRequest.Issuer,
                audience: tokenRequest.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }

    public class TokenRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class TokenCreateRequest : TokenRequest
    {
        public required string UserId { get; set; }
        public required string RoleName { get; set; }
        public required string SecretKey { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
    }
}
