using Carter;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Ordering.API.EventHandlers;
using Ordering.Infrastructure.Data;
using Serilog;
using BuildingBlocks.Messaging; // Namespace chứa hàm AddMessageBroker

var builder = WebApplication.CreateBuilder(args);

// 1. Setup Serilog (Logging)
builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

// 2. Setup Application Services (MediatR)
// Lưu ý: Cần có class AssemblyReference trong Ordering.Application
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Ordering.Application.AssemblyReference).Assembly);
});

// 3. Setup Infrastructure Services (Database)
builder.Services.AddDbContext<OrderingDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

// 4. Setup Messaging (RabbitMQ via MassTransit)
// Truyền Assembly chứa Consumer (BasketCheckoutConsumer) để MassTransit tự scan
builder.Services.AddMessageBroker(builder.Configuration, typeof(BasketCheckoutConsumer).Assembly);

// 5. Setup API Services
builder.Services.AddCarter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 6. Auto Migration (Tự động tạo Database/Tables)
// Chỉ dùng cho môi trường Development
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<OrderingDbContext>();
        // Tự động apply các migration chưa chạy
        context.Database.Migrate();
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Map các endpoints (nếu có)
app.MapCarter();

app.Run();