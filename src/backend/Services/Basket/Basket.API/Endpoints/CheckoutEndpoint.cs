using Basket.Application.Data;
using Basket.Domain.Entities;
using BuildingBlocks.Messaging.Events;
using Carter;
using Mapster;
using MassTransit;


namespace Basket.API.Endpoints
{
    public record CheckoutRequest(string UserName, string FirstName, string LastName, string EmailAddress);

    public class CheckoutEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket/checkout", async (CheckoutRequest request, IBasketRepository repository, IPublishEndpoint publishEndpoint) =>
            {
                // 1. Lấy giỏ hàng
                var basket = await repository.GetBasket(request.UserName);
                if (basket == null)
                    return Results.NotFound("Basket not found");

                // 2. Map dữ liệu sang Event Message
                var eventMessage = request.Adapt<BasketCheckoutEvent>();
                eventMessage.TotalPrice = basket.TotalPrice;

                // 3. Bắn tin nhắn lên RabbitMQ (Quan trọng nhất)
                await publishEndpoint.Publish(eventMessage);

                // 4. Xóa giỏ hàng trong Redis (vì đã checkout rồi)
                await repository.DeleteBasket(request.UserName);

                return Results.Accepted(); // Trả về 202 Accepted (Đã nhận, đang xử lý ngầm)
            })
            .WithName("CheckoutBasket")
            .WithTags("Basket");
        }
    }
}