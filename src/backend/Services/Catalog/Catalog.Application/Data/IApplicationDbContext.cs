using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Data
{
    public interface IApplicationDbContext
    {
        // Chỉ khai báo những DbSet nào mà Application cần thao tác
        DbSet<Product> Products { get; }

        // Hàm lưu thay đổi xuống DB
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}