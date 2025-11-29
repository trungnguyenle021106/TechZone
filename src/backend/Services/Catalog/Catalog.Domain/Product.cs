// Product.cs
namespace Catalog.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        // Sau này sẽ thêm Category, Brand sau. Keep it simple first.
    }
}