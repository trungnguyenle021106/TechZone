using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;

namespace Ordering.Application.Data
{
    public interface IApplicationDbContext
    {
        // Chỉ khai báo những DbSet nào mà Application cần thao tác
        DbSet<Order> Orders { get; }
        DbSet<OrderItem> OrderItems { get; }

        // Hàm lưu thay đổi xuống DB
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
