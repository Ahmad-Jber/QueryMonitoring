using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Diagnostics.dotMemory;
using BenchmarkDotNet.Diagnostics.dotTrace;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Microsoft.EntityFrameworkCore;
using Perfolizer.Mathematics.OutlierDetection;
using QueryMonitoring.DatabaseManagement.DbContexts;
using QueryMonitoring.DatabaseManagement.Interceptors;
using QueryMonitoring.Enums;
using QueryMonitoring.Products.Entities;
using QueryMonitoring.Shops;

namespace QueryMonitoring.DatabaseManagement.Repositories;
[Outliers(OutlierMode.DontRemove)]
[SimpleJob(RunStrategy.ColdStart)]
[MemoryDiagnoser]
[
    RankColumn,
    MinColumn,
    MaxColumn,
    Q1Column,
    Q3Column,
    AllStatisticsColumn,
]
[JsonExporterAttribute.Full, CsvMeasurementsExporter, CsvExporter(CsvSeparator.Comma), HtmlExporter,
 MarkdownExporterAttribute.GitHub]
[GcServer(true)]
[JsonExporterAttribute.Full()]
[DotMemoryDiagnoser]
[DotTraceDiagnoser]
[RPlotExporter]
[ReturnValueValidator(true)]
public class ProductRepository: IProductRepository
{
    AlQimaDbContext alQimaDbContext;
    RamallahDbContext ramallahDbContext;
    public ProductRepository()
    {
    }

    public ProductRepository(AlQimaDbContext alQimaDbContext,
        RamallahDbContext ramallahDbContext)
    {
        this.ramallahDbContext = ramallahDbContext;
        this.alQimaDbContext = alQimaDbContext;
    }
    public async Task<IList<Product>> GetAllProducts()
    {
        IList<Product> result;
        result = (await alQimaDbContext.Products.Include(e=>e.Shop).ToListAsync())
            .Union(await ramallahDbContext.Products.Include(e=>e.Shop).ToListAsync()).ToList();
        return result;
    }
    [Benchmark]
    [Arguments(10)]
    public async Task SeedProducts(int count)
    {
        var qimaProducts = new List<Product>();
        var ramallahProducts = new List<Product>();
        for (var i = 0; i < count; ++i)
        {
            try
            {
                var qimaProductType = new ProductType
                {
                    Name = $"Product {i}",
                    PriceCategory = PriceCategoryEnum.Count,
                };
                var ramallahProductType = new ProductType
                {
                    Name = $"Product {i}",
                    PriceCategory = PriceCategoryEnum.Count,
                };
                await alQimaDbContext.ProductTypes.AddAsync(qimaProductType);
                await alQimaDbContext.SaveChangesAsync();
                await ramallahDbContext.ProductTypes.AddAsync(ramallahProductType);
                await ramallahDbContext.SaveChangesAsync();
                var qimaShop = new Shop()
                {
                    Name = $"Shop {i}",
                    Address = $"Shop {i}",
                };
                var ramallahShop = new Shop
                {
                    Name = $"Shop {i}",
                    Address = $"Shop {i}",
                };
                await alQimaDbContext.Shops.AddAsync(qimaShop);
                await alQimaDbContext.SaveChangesAsync();
                await ramallahDbContext.Shops.AddAsync(ramallahShop);
                await ramallahDbContext.SaveChangesAsync();
                qimaProducts.Add(new Product
                {
                    ProductType = qimaProductType,
                    Name = $"Product {i}",
                    Manufacturer = $"Manufacturer {i}",
                    Quality = QualityEnum.Excellent,
                    Size = 0.3f,
                    Price = 300,
                    Database = DatabaseEnum.Both,
                    Quantity = 50,
                    Shop = qimaShop,
                });
                ramallahProducts.Add(new Product
                {
                    ProductType = ramallahProductType,
                    Name = $"Product {i}",
                    Manufacturer = $"Manufacturer {i}",
                    Quality = QualityEnum.Excellent,
                    Size = 0.3f,
                    Price = 300,
                    Database = DatabaseEnum.Both,
                    Quantity = 50,
                    Shop = ramallahShop,
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        await alQimaDbContext.AddRangeAsync(qimaProducts);
        await ramallahDbContext.AddRangeAsync(ramallahProducts);
        await alQimaDbContext.SaveChangesAsync();
        await ramallahDbContext.SaveChangesAsync();
    }

    public async Task TransferProduct(Guid productId)
    {
        var product = await alQimaDbContext.Products
            .Include(e => e.Shop)
            .Include(e => e.ProductType)
            .SingleOrDefaultAsync(e => e.Id == productId);
        if (product == null)
            return;
        await ramallahDbContext.Products.AddAsync(product);
        await ramallahDbContext.SaveChangesAsync();
        alQimaDbContext.Remove(productId);
        await alQimaDbContext.SaveChangesAsync();
    }

    public async Task AddProduct(Product product)
    {
        if (product.Database == DatabaseEnum.Ramallah)
        {
            await ramallahDbContext.Products.AddAsync(product);
            await ramallahDbContext.SaveChangesAsync();
            Console.WriteLine("Total duration time: " +
                              MallDbInterceptor.Durations.Aggregate((sum, next) => sum + next));
            MallDbInterceptor.Durations.Clear();
        }

        if (product.Database == DatabaseEnum.AlQima)
        {
            await alQimaDbContext.Products.AddAsync(product);
            await alQimaDbContext.SaveChangesAsync();
            Console.WriteLine("Total duration time: " +
                              MallDbInterceptor.Durations.Aggregate((sum, next) => sum + next));
            MallDbInterceptor.Durations.Clear();
        }

        await ramallahDbContext.Products.AddAsync(product);
        await ramallahDbContext.SaveChangesAsync();
        await alQimaDbContext.Products.AddAsync(product);
        await alQimaDbContext.SaveChangesAsync();
    }

    public async Task UpdateProduct(Product product)
    {
        var updatedProduct = new Product(product);
        if (product.Database == DatabaseEnum.Ramallah)
        {
            ramallahDbContext.Update(updatedProduct);
            await ramallahDbContext.SaveChangesAsync();
            Console.WriteLine("Total duration time: " +
                              MallDbInterceptor.Durations.Aggregate((sum, next) => sum + next));
            MallDbInterceptor.Durations.Clear();
        }

        if (product.Database == DatabaseEnum.AlQima)
        {
            alQimaDbContext.Products.Update(updatedProduct);
            await alQimaDbContext.SaveChangesAsync();
            Console.WriteLine("Total duration time: " +
                              MallDbInterceptor.Durations.Aggregate((sum, next) => sum + next));
            MallDbInterceptor.Durations.Clear();
        }

        ramallahDbContext.Update(updatedProduct);
        await ramallahDbContext.SaveChangesAsync();
        alQimaDbContext.Products.Update(updatedProduct);
        await alQimaDbContext.SaveChangesAsync();
    }
}