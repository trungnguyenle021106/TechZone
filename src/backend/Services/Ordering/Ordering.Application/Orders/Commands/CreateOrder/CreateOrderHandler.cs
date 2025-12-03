using BuildingBlocks.Core.CQRS;
using Ordering.Domain.Entities;
using Ordering.Domain.ValueObjects; // Import namespace chứa Address Domain
using Ordering.Infrastructure.Data;

namespace Ordering.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderHandler : ICommandHandler<CreateOrderCommand, CreateOrderResult>
    {
        private readonly OrderingDbContext _context;

        public CreateOrderHandler(OrderingDbContext context)
        {
            _context = context;
        }

        public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            // 1. Map AddressDto -> Address (Domain Value Object)
            // QUAN TRỌNG: Phải dùng từ khóa "new" để tạo instance mới gắn liền với Order này
            var shippingAddress = new Address(
                command.Order.ShippingAddress.FirstName,
                command.Order.ShippingAddress.LastName,
                command.Order.ShippingAddress.EmailAddress,
                command.Order.ShippingAddress.AddressLine,
                command.Order.ShippingAddress.Country,
                command.Order.ShippingAddress.ZipCode
            );

            var billingAddress = new Address(
                command.Order.BillingAddress.FirstName,
                command.Order.BillingAddress.LastName,
                command.Order.BillingAddress.EmailAddress,
                command.Order.BillingAddress.AddressLine,
                command.Order.BillingAddress.Country,
                command.Order.BillingAddress.ZipCode
            );

            // 2. Tạo Order
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = command.Order.CustomerId,
                OrderName = command.Order.OrderName,
                ShippingAddress = shippingAddress, // Gán instance vừa new
                BillingAddress = billingAddress,   // Gán instance vừa new
                Status = "Pending"
            };

            // 3. Thêm Items
            foreach (var itemDto in command.Order.OrderItems)
            {
                order.AddOrderItem(itemDto.ProductName, itemDto.Price, itemDto.Quantity);
            }

            // 4. Save DB
            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateOrderResult(order.Id);
        }
    }
}