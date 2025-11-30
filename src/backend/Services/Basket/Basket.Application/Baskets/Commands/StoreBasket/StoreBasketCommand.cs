using BuildingBlocks.Core.CQRS;
using Basket.Domain.Entities;
using FluentValidation;

namespace Basket.Application.Baskets.Commands.StoreBasket
{
    public record StoreBasketResult(string UserName);
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;

    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull();
            RuleFor(x => x.Cart.UserName).NotEmpty();
        }
    }
}