using BuildingBlocks.Messaging;
using Basket.Application.Data;
using Basket.Infrastructure.Data;
using Carter;
using Serilog;

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCarter();
app.UseHttpsRedirection();
app.Run();