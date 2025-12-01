using Identity.Application.Common.Interfaces;
using Identity.Application.Services;
using Identity.Domain.Models;
using Identity.Infrastructure.Data;
using Identity.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Carter;

var builder = WebApplication.CreateBuilder(args);

// 1. Add DbContext
builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

// 2. Add Identity (User, Role)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppIdentityDbContext>()
    .AddDefaultTokenProviders();

// 3. Đăng ký Services của mình
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

// 4. Config Carter & Swagger
builder.Services.AddCarter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 5. Auto Migration (Tạo bảng AspNetUsers...)
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
        context.Database.Migrate();
    }
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCarter();
app.Run();