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
        public static List<TokenCreateRequest> Users { get; set; } = new List<TokenCreateRequest>
        {
            new TokenCreateRequest() { UserId = Guid.NewGuid().ToString(), Username="renato", Password="renato", RoleName = "User" },
            new TokenCreateRequest() { UserId = Guid.NewGuid().ToString(), Username="admin", Password="admin", RoleName = "Admin" }
        };

        public APIGatewayProxyResponse FunctionHandler(TokenRequest request, ILambdaContext context)
        {
            // Bad request when request is null or invalid
            if (request == null || !request.isValid())
            {
                return CreateGatewayResponse(400, JsonSerializer.Serialize(new { Message = "Request is not valid" }));
            }

            // Not Authorized when user is not found
            var user = Users.Find(u => u.Username == request.Username && u.Password == request.Password);
            if (user == null)
            {
                return CreateGatewayResponse(401, JsonSerializer.Serialize(new { Message = "Unauthorized" }));
            }
            
            // Generate token when user is found
            var tcr = new TokenCreateRequest
            {
                Username = request.Username,
                Password = request.Password,
                UserId = "1",
                RoleName = "Admin",
                SecretKey = "33a19758-19c0-4be4-9582-f010eb7928f4",
                Issuer = "https://aws-hosting-strategies.com",
                Audience = "https://gtw-token-api.aws-hosting-strategies.com"
            };

            return  CreateGatewayResponse(200, GenerateToken(tcr));
        }

        private string GenerateToken(TokenCreateRequest tokenRequest)
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

        private APIGatewayProxyResponse CreateGatewayResponse(int statusCode, string body)
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = statusCode,
                Body = body,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
    }

    public class TokenRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
         // to be a valid request, both username and password must be provided
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
