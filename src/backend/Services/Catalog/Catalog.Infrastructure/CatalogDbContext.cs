using Catalog.Application.Data; // Reference tới Interface vừa tạo
using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Catalog.Infrastructure.Data
{
    // Kế thừa cả DbContext (của EF) VÀ IApplicationDbContext (của App)
    public class CatalogDbContext : DbContext, IApplicationDbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Apply tất cả các config (ProductConfiguration) nằm trong Assembly này
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }

        // Không cần implement SaveChangesAsync thủ công vì lớp cha DbContext đã có sẵn rồi, 
        // nó tự động khớp với Interface.
    }
}