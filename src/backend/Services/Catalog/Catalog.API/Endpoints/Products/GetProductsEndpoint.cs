using Carter;
using MediatR;
using Catalog.Application.Products.Queries.GetProducts;
using Mapster;

namespace Catalog.API.Endpoints.Products
{
    // Response DTO (Optional: Nếu muốn trả về định dạng khác Entity)
    public record GetProductsResponse(IEnumerable<ProductDto> Products);
    public record ProductDto(Guid Id, string Name, string Description, decimal Price);

    public class GetProductsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async (ISender sender) =>
            {
                var result = await sender.Send(new GetProductsQuery());

                // Map từ Result (Entity) sang Response (DTO)
                // Mapster đủ thông minh để map list sang list
                var response = result.Adapt<GetProductsResponse>();

                return Results.Ok(response);
            })
            .WithName("GetProducts")
            .WithSummary("Get products")
            .WithDescription("Get products list");
        }
    }
}