using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using QueryMonitoring.Entities;
using QueryMonitoring.Products.Entities;
using QueryMonitoring.Shops;
using BenchmarkDotNet.Diagnostics.dotMemory;
using BenchmarkDotNet.Diagnostics.dotTrace;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Exporters.Csv;
using Perfolizer.Mathematics.OutlierDetection;

namespace QueryMonitoring.DatabaseManagement.DbContexts;
[Outliers(OutlierMode.DontRemove)]
[SimpleJob(RunStrategy.Monitoring)] 
[
    RankColumn,
    MinColumn,
    MaxColumn,
    BaselineColumn,
    CategoriesColumn,
    IterationsColumn,
    NamespaceColumn,
    AllStatisticsColumn,
]
[
    CsvMeasurementsExporter,
    CsvExporter(CsvSeparator.Comma),
    HtmlExporter,
    PlainExporter,
]   
[MemoryDiagnoser]
[DotMemoryDiagnoser]
[DotTraceDiagnoser]
[RPlotExporter]
public class RamallahDbContext : DbContext
{
    public RamallahDbContext(DbContextOptions<RamallahDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<ProductType> ProductTypes { get; set; }
    public DbSet<Shop> Shops { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    
    
    [Benchmark]
    public Task SaveChangesAsync()
    {
        return SaveChangesAsync(CancellationToken.None);
    }
}