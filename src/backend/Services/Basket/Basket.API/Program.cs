using Basket.Application.Data;
using Basket.Infrastructure.Data;
using BuildingBlocks.Messaging;
using Carter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Serilog
builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

// 2. Add Services
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Basket.Application.AssemblyReference).Assembly);
});

// 3. Redis Configuration (QUAN TRỌNG)
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

// 4. DI Repository
// Đăng ký implementation BasketRepository cho interface IBasketRepository
builder.Services.AddScoped<IBasketRepository, BasketRepository>();

// Thêm dòng này để đăng ký MassTransit
builder.Services.AddMessageBroker(builder.Configuration);

// === BẮT ĐẦU CẤU HÌNH JWT ===
var secretKey = builder.Configuration["JwtSettings:Secret"];
var key = Encoding.UTF8.GetBytes(secretKey!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
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
builder.Services.AddAuthorization(); // Thêm dòng này
// === KẾT THÚC CẤU HÌNH JWT ===

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

// === KÍCH HOẠT MIDDLEWARE (Đặt trước MapCarter) ===
app.UseAuthentication(); // Bắt buộc: "Anh là ai?"
app.UseAuthorization();  // Bắt buộc: "Anh được làm gì?"

app.MapCarter();
app.Run();