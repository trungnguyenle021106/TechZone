using Basket.Application.Data;
using BuildingBlocks.Core.CQRS;

namespace Basket.Application.Baskets.Queries.GetBasket
{
    public class GetBasketHandler : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        private readonly IBasketRepository _repository;
        public GetBasketHandler(IBasketRepository repository) => _repository = repository;

        public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
        {
            var basket = await _repository.GetBasket(query.UserName, cancellationToken);
            return new GetBasketResult(basket);
        }
    }
}