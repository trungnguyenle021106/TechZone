using BuildingBlocks.Core.CQRS;
using Basket.Domain.Entities;

namespace Basket.Application.Baskets.Queries.GetBasket
{
    public record GetBasketResult(ShoppingCart Cart);
    public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;
}