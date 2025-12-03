using Basket.Application.Baskets.Commands.StoreBasket;
using Basket.Application.Baskets.Queries.GetBasket;
using Basket.Domain.Entities;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Basket.API.Endpoints
{
    // Request DTO
    public record StoreBasketRequest(ShoppingCart Cart);
    public record StoreBasketResponse(string UserName);
    public record GetBasketResponse(ShoppingCart Cart);

    public class BasketEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/basket").WithTags("Basket");

            // 1. Get Basket (Không cần truyền userName trên URL nữa)
            // Client chỉ cần gọi GET /basket và kẹp Token
            group.MapGet("/", async (ISender sender, ClaimsPrincipal user) =>
            {
                // Lấy UserName từ Token (ClaimTypes.Name)
                var userName = user.Identity?.Name
                   ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? user.FindFirst("sub")?.Value
                   ?? user.FindFirst("name")?.Value;

                if (string.IsNullOrEmpty(userName))
                {
                    return Results.Unauthorized();
                }

                var result = await sender.Send(new GetBasketQuery(userName));
                var response = result.Adapt<GetBasketResponse>();
                return Results.Ok(response);
            })
            .RequireAuthorization(); // Bắt buộc phải có Token mới gọi được hàm này

            // 2. Store Basket
            group.MapPost("/", async (StoreBasketRequest request, ISender sender, ClaimsPrincipal user) =>
            {
                var userName = user.Identity?.Name
                  ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                  ?? user.FindFirst("sub")?.Value
                  ?? user.FindFirst("name")?.Value;

                if (string.IsNullOrEmpty(userName)) return Results.Unauthorized();

                // Override userName trong request bằng userName thật từ Token để bảo mật
                // (Tránh trường hợp Token là ông A mà lại gửi request sửa giỏ hàng ông B)
                request.Cart.UserName = userName;

                var command = request.Adapt<StoreBasketCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<StoreBasketResponse>();
                return Results.Created($"/basket/{response.UserName}", response);
            })
            .RequireAuthorization();
        }
    }
}