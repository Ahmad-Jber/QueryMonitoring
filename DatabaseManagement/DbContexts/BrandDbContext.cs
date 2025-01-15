using Microsoft.EntityFrameworkCore;
using QueryMonitoring.Entities;
using QueryMonitoring.Products.Entities;
using QueryMonitoring.Shops;
namespace QueryMonitoring.DatabaseManagement.DbContexts;

public class BrandDbContext: DbContext
{
    public BrandDbContext(DbContextOptions<BrandDbContext> options) : base(options)
    {
    }

    DbSet<Product> Products { get; set; }
    DbSet<ProductType> ProductTypes { get; set; }
    DbSet<Shop> Shops { get; set; }
    DbSet<Floor> Floors { get; set; }
    DbSet<Attachment> Attachments { get; set; }
}