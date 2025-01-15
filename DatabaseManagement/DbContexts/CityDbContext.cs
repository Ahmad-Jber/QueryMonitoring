using Microsoft.EntityFrameworkCore;
using QueryMonitoring.Entities;
using QueryMonitoring.Products.Entities;
using QueryMonitoring.Shops;
namespace QueryMonitoring.DatabaseManagement.DbContexts;

public class CityDbContext: DbContext
{
    public CityDbContext(DbContextOptions<CityDbContext> options) : base(options)
    {
    }

    DbSet<Product> Products { get; set; }
    DbSet<ProductType> ProductTypes { get; set; }
    DbSet<Shop> Shops { get; set; }
    DbSet<Floor> Floors { get; set; }
    DbSet<Attachment> Attachments { get; set; }
}