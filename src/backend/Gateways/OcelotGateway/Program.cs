using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// 1. Thêm Authentication Service
var secretKey = builder.Configuration["JwtSettings:Secret"];
var key = Encoding.UTF8.GetBytes(secretKey!);

builder.Services.AddAuthentication()
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            ClockSkew = TimeSpan.Zero
        };
    });


// Add CORS Service
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://localhost:4200"); // Cho phép Angular gọi vào
    });
});

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

// Use CORS Middleware (Phải đặt trước UseAuthentication và UseOcelot)
app.UseCors("CorsPolicy");

// 2. Kích hoạt Authentication Middleware (Trước UseOcelot)
app.UseAuthentication();
app.UseAuthorization(); // (Optional nhưng nên có)

await app.UseOcelot();

app.Run();