using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Amazon.Lambda.Core;
using Microsoft.IdentityModel.Tokens;
using Amazon.Lambda.APIGatewayEvents;
using System.Text.Json;


// Lambda function handler class

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace TokenGeneratorLambda
{
    // Lambda function handler
    public class Function
    {
        public static List<TokenCreateRequest> Users = new List<TokenCreateRequest>
        {
            new TokenCreateRequest() { UserId = Guid.NewGuid().ToString(), Username="renato", Password="renato", RoleName = "User" },
            new TokenCreateRequest() { UserId = Guid.NewGuid().ToString(), Username="admin", Password="admin", RoleName = "Admin" }
        };
        
        public APIGatewayProxyResponse FunctionHandler(TokenRequest request, ILambdaContext context)
        {
            if (request == null || !request.isValid())
            {
                throw new ArgumentNullException("Request is not valid.");
            }

            var user = Users.FirstOrDefault(u => u.Username == request.Username && u.Password == request.Password);
            if (user == null)
            {
                return UnauthorizedResponse();
            }
            
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

            return new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Body = GenerateToken(tcr),
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };            
        }

        public string GenerateToken(TokenCreateRequest tokenRequest)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenRequest.SecretKey ?? ""));
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

        private APIGatewayProxyResponse UnauthorizedResponse()
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = 401,
                Body = JsonSerializer.Serialize(new { Message = "Unauthorized" }),
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
    }

    public class TokenRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
         public bool isValid() 
        {
            return !string.IsNullOrEmpty(this.Username) && !string.IsNullOrEmpty(this.Password);
        }
    }

    public class TokenCreateRequest : TokenRequest
    {
        public required string UserId { get; set; }
        public required string RoleName { get; set; }
        public string? SecretKey { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
    }    
}
