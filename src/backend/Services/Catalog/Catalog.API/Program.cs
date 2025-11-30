using Carter; 
using Catalog.Application.Data;
using Catalog.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// 1. Setup Serilog
builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

// 2. Setup Services
// Thay vì AddControllers, ta dùng AddCarter
builder.Services.AddCarter();

// Add MediatR (Chúng ta sẽ cần cái này để bắn Command sang Application Layer)
// Lưu ý: Cần reference project Catalog.Application để trỏ tới Assembly chứa Handler
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Catalog.Application.AssemblyReference).Assembly);
    // *Lưu ý*: Bạn cần tạo 1 class rỗng tên AssemblyReference trong Catalog.Application để dòng này chạy, 
    // hoặc trỏ vào 1 class bất kỳ trong Application layer.
});

// Đăng ký tất cả Validators nằm trong Assembly của Catalog.Application
builder.Services.AddValidatorsFromAssembly(typeof(Catalog.Application.AssemblyReference).Assembly);

// 1. Đăng ký DbContext như bình thường (để có Connection String)
builder.Services.AddDbContext<CatalogDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

// 2. Đăng ký Interface mapping (QUAN TRỌNG)
// Ý nghĩa: Khi Inject IApplicationDbContext, hãy lấy instance của CatalogDbContext
builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<CatalogDbContext>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 3. Auto Migration
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
    context.Database.Migrate();
}

// 4. Configure Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// MapCarter sẽ tự động tìm tất cả các class kế thừa ICarterModule để đăng ký route
app.MapCarter();

app.UseHttpsRedirection(); // Nên có
// app.UseAuthorization(); // Tạm thời chưa có Auth, có thể comment lại

app.Run();