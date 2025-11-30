using BuildingBlocks.Core.CQRS;
using Catalog.Domain.Entities;

namespace Catalog.Application.Products.Queries.GetProducts
{
    // 1. Result (Output)
    // Để đơn giản, ta trả về danh sách Product Entity luôn (hoặc tạo DTO nếu muốn ẩn field)
    // IEnumerable dùng cho danh sách chỉ đọc, nhẹ hơn List
    public record GetProductsResult(IEnumerable<Product> Products);

    // 2. Query (Input)
    // Tạm thời lấy tất cả, sau này thêm Pagination (phân trang) vào đây sau
    public record GetProductsQuery() : IQuery<GetProductsResult>;
}