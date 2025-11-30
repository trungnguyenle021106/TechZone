using BuildingBlocks.Core.CQRS;
using Catalog.Application.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Products.Queries.GetProducts
{
    public class GetProductsHandler
        : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetProductsHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            // Tối ưu hóa: AsNoTracking() thường do EF theo dõi sự thay đổi của object nhưng đọc dữ liệu thì không cần,
            // nếu không tắt gây tốn tài nguyên
            var products = await _dbContext.Products
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .ToListAsync(cancellationToken);

            return new GetProductsResult(products);
        }
    }
}