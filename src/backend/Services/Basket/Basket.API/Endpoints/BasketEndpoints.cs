using Basket.Application.Baskets.Commands.StoreBasket;
using Basket.Application.Baskets.Queries.GetBasket;
using Basket.Domain.Entities;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

            // Get Basket
            group.MapGet("/{userName}", async (string userName, ISender sender) =>
            {
                var result = await sender.Send(new GetBasketQuery(userName));
                var response = result.Adapt<GetBasketResponse>();
                return Results.Ok(response);
            });

            // Store Basket
            group.MapPost("/", async (StoreBasketRequest request, ISender sender) =>
            {
                var command = request.Adapt<StoreBasketCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<StoreBasketResponse>();
                return Results.Created($"/basket/{response.UserName}", response);
            });
        }
    }
}