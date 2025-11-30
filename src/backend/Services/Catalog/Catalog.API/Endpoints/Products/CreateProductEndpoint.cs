using Carter;
using MediatR;
using Catalog.Application.Products.Commands.CreateProduct;
using Mapster;

namespace Catalog.API.Endpoints.Products
{
    // Request Body (Cái người dùng gửi lên)
    public record CreateProductRequest(string Name, string Description, decimal Price);

    // Response Body (Cái trả về)
    public record CreateProductResponse(Guid Id);

    public class CreateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
            {
                // 1. Map Request -> Command (Dùng Mapster)
                var command = request.Adapt<CreateProductCommand>();

                // 2. Gửi Command đi xử lý qua MediatR
                // MediatR sẽ tự tìm Handler phù hợp để chạy
                var result = await sender.Send(command);

                // 3. Map Result -> Response
                var response = result.Adapt<CreateProductResponse>();

                return Results.Created($"/products/{response.Id}", response);
            })
            .WithName("CreateProduct")
            .WithSummary("Create a new product")
            .WithDescription("Create a new product into the catalog");
        }
    }
}