using Basket.Application.Data;
using BuildingBlocks.Core.CQRS;

namespace Basket.Application.Baskets.Commands.StoreBasket
{
    public class StoreBasketHandler : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        private readonly IBasketRepository _repository;
        public StoreBasketHandler(IBasketRepository repository) => _repository = repository;

        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            await _repository.StoreBasket(command.Cart, cancellationToken);
            return new StoreBasketResult(command.Cart.UserName);
        }
    }
}