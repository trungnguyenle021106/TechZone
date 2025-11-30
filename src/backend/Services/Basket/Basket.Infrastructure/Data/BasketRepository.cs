using Basket.Application.Data;
using Basket.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed; // Thư viện cache chuẩn của .NET
using System.Text.Json;

namespace Basket.Infrastructure.Data
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            var basket = await _redisCache.GetStringAsync(userName, cancellationToken);

            if (string.IsNullOrEmpty(basket))
                return new ShoppingCart(userName); // Nếu chưa có thì trả về giỏ mới

            return JsonSerializer.Deserialize<ShoppingCart>(basket)!;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            // Serialize Object -> String
            var json = JsonSerializer.Serialize(basket);

            // Lưu vào Redis (Key = UserName)
            await _redisCache.SetStringAsync(basket.UserName, json, cancellationToken);

            return basket;
        }

        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            await _redisCache.RemoveAsync(userName, cancellationToken);
            return true;
        }
    }
}