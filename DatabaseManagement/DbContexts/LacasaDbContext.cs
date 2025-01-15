using Microsoft.EntityFrameworkCore;
using QueryMonitoring.Entities;
using QueryMonitoring.Enums;
using QueryMonitoring.Products.Entities;
using QueryMonitoring.Shops;
namespace QueryMonitoring.DatabaseManagement.DbContexts;

public class LacasaDbContext: DbContext
{
    public LacasaDbContext(DbContextOptions<LacasaDbContext> options) : base(options)
    {
    }

    DbSet<Product> Products { get; set; }
    DbSet<ProductType> ProductTypes { get; set; }
    DbSet<Shop> Shops { get; set; }
    DbSet<Attachment> Attachments { get; set; }
}