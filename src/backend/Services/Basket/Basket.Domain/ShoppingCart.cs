namespace Basket.Domain.Entities
{
    public class ShoppingCart
    {
        public string UserName { get; set; } = default!;
        public List<ShoppingCartItem> Items { get; set; } = new();

        public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);

        public ShoppingCart(string userName)
        {
            UserName = userName;
        }

        // Constructor cho mapping (cần thiết khi deserialize)
        public ShoppingCart() { }
    }
}