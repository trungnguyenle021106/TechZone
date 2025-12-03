namespace BuildingBlocks.Messaging.Events
{
    // Integration Event thường dùng quá khứ đơn (Checkout-ed)
    public record BasketCheckoutEvent
    {
        public string UserName { get; set; } = default!;
        public decimal TotalPrice { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string EmailAddress { get; set; } = default!;
        // ... thêm địa chỉ shipping nếu cần
    }
}