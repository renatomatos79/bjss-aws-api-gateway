using System.Text;
using eventsapi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddScoped<ITokenService, TokenService>(sp =>
{
    return new TokenService(
        secretKey: builder.GetSecretKey(),
        issuer: builder.GetSecretIssuer(),
        audience: builder.GetSecretAudience()
    );
});

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.GetSecretIssuer(),
            ValidAudience = builder.GetSecretAudience(),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.GetSecretKey()))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Authenticated", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Login API", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

// Configure Kestrel to use HTTPS and require client certificates
builder.WebHost.ConfigureKestrel(options =>
{
    options.ConfigureHttpsDefaults(httpsOptions =>
    {
        httpsOptions.ClientCertificateMode = ClientCertificateMode.RequireCertificate;

        // Add client certificate validation
        httpsOptions.ClientCertificateValidation = (certificate, chain, sslPolicyErrors) =>
        {
            // Custom validation logic for client certificate
            if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
            {
                // If there are no SSL policy errors, accept the certificate
                return true;
            }

            // Optionally, inspect certificate fields for additional validation
            Console.WriteLine($"Certificate Content");
            Console.WriteLine($"Client Certificate Subject: {certificate.Subject}");
            Console.WriteLine($"Issuer: {certificate.Issuer}");
            Console.WriteLine($"Expiration: {certificate.GetExpirationDateString()}");

            // Example: only allow certificates issued by a specific CA
            //if (certificate.Issuer.Contains("CN=ec2.aws-hosting-strategies.com"))
            //{
            //    return true;
            //}

            return true;
        };
    });
});



var app = builder.Build();

app.UseHttpsRedirection();

// Middleware to check if the client certificate is present and valid
app.Use(async (context, next) =>
{
    var cert = context.Connection.ClientCertificate;

    if (cert == null)
    {
        context.Response.StatusCode = 400; // Bad request
        await context.Response.WriteAsync("Client certificate is required.");
        return;
    }

    await next();
});

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
