using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Configuration (Đọc file ocelot.json)
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// 2. Add Ocelot Service
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

// 3. Use Ocelot Middleware
// Phải dùng await vì Ocelot chạy bất đồng bộ
await app.UseOcelot();

app.Run();