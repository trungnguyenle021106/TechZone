using BuildingBlocks.Core.CQRS;

namespace Ordering.Application.Orders.Commands.CreateOrder
{
    public record CreateOrderResult(Guid Id);

    public record CreateOrderCommand(OrderDto Order) : ICommand<CreateOrderResult>;

    public record OrderDto(
        Guid CustomerId,
        string OrderName,
        AddressDto ShippingAddress, // Dùng DTO
        AddressDto BillingAddress,  // Dùng DTO
        List<OrderItemDto> OrderItems
    );

    // DTO thuần túy, không dính logic domain
    public record AddressDto(
        string FirstName,
        string LastName,
        string EmailAddress,
        string AddressLine,
        string Country,
        string ZipCode
    );

    public record OrderItemDto(string ProductName, decimal Price, int Quantity);
}