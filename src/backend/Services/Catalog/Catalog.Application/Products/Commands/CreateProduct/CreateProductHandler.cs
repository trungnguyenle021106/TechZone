using BuildingBlocks.Core.CQRS;
using Catalog.Application.Data; // Dùng Interface
using Catalog.Domain.Entities;
using MediatR;

namespace Catalog.Application.Products.Commands.CreateProduct
{
    public class CreateProductHandler
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        // Dependency Injection: Chỉ phụ thuộc vào Interface
        private readonly IApplicationDbContext _dbContext;

        public CreateProductHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
                Description = command.Description,
                Price = command.Price
                // Các field khác...
            };

            // Thao tác qua Interface, code không hề biết bên dưới là SQL Server hay gì
            _dbContext.Products.Add(product);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CreateProductResult(product.Id);
        }
    }
}