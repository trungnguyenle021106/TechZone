using Ordering.Domain.ValueObjects;

namespace Ordering.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; } // Giả lập, sau này lấy từ Identity
        public string OrderName { get; set; } = default!; // Tên gợi nhớ (ví dụ: userName)

        public Address ShippingAddress { get; set; } = default!;
        public Address BillingAddress { get; set; } = default!;

        public string Status { get; set; } = "Pending"; // Đơn giản hóa state
        public decimal TotalPrice { get; set; }

        // Relationship
        public List<OrderItem> OrderItems { get; set; } = new();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Domain Method: Thêm item vào đơn
        public void AddOrderItem(string productName, decimal price, int quantity)
        {
            var item = new OrderItem(productName, price, quantity);
            OrderItems.Add(item);
        }
    }
}