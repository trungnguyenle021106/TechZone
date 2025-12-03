using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using System.Reflection;

namespace BuildingBlocks.Messaging
{
    public static class MassTransitExtensions
    {
        public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
        {
            services.AddMassTransit(config =>
            {
                // Tự động tìm các Consumer (người nhận) trong Assembly được truyền vào
                config.SetKebabCaseEndpointNameFormatter();

                if (assembly != null)
                    config.AddConsumers(assembly);

                config.UsingRabbitMq((context, configurator) =>
                {
                    // Lấy cấu hình từ appsettings.json
                    configurator.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                    {
                        host.Username(configuration["MessageBroker:UserName"]!);
                        host.Password(configuration["MessageBroker:Password"]!);
                    });

                    // Cấu hình Queue nhận tin nhắn
                    configurator.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}