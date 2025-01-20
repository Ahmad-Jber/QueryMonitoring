using BenchmarkDotNet.Attributes;
using QueryMonitoring.Enums;
using QueryMonitoring.Products.Entities;

namespace QueryMonitoring.DatabaseManagement.Repositories;

public interface IProductRepository
{
    Task<IList<Product>> GetAllProducts();
    Task SeedProducts(int count);
    Task TransferProduct(Guid productId);
    Task AddProduct(Product product);
    Task UpdateProduct(Product product);
}