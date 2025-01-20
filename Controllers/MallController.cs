using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using QueryMonitoring.Consts;
using QueryMonitoring.DatabaseManagement.Interceptors;
using QueryMonitoring.DatabaseManagement.Repositories;
using QueryMonitoring.Dto;
using QueryMonitoring.Products.Entities;

namespace QueryMonitoring.Controllers;

[ApiController]
[Route("api/[controller]/[Action]")]
public class MallController(
    IProductRepository productRepository,
    IMemoryCache memoryCache
) : Controller
{

    [HttpGet]
    public async Task<ActionResult<(IList<Product>, DbMonitoringDto)>> GetAllProducts()
    {
        var result = await productRepository.GetAllProducts();
        var TotalDurationTime = MallDbInterceptor.Durations.Aggregate((z, e) => z + e);
        Console.WriteLine("Total duration time: " +
                          TotalDurationTime);
        MallDbInterceptor.Durations.Clear();
        return Ok(new
        {
            Products = result.FirstOrDefault(),
            Monitoring = memoryCache.Get(InterceptorConsts.MonitoringCacheKey) ?? null,
            TotalDurationTime
        });
    }

    [HttpGet]
    public async Task<ActionResult<DbMonitoringDto>> SeedProducts([FromQuery] int count)
    {
        
        await productRepository.SeedProducts(count);
        var TotalDurationTime = MallDbInterceptor.Durations.Aggregate((z, e) => z + e);
        Console.WriteLine("Total duration time: " +
                          TotalDurationTime);
        MallDbInterceptor.Durations.Clear();
        return Ok(
            new
            {
                Monitoring = memoryCache.Get(InterceptorConsts.MonitoringCacheKey) ?? null,
                TotalDurationTime
            });
    }

    [HttpPost]
    public async Task<IActionResult> TransferProduct(Guid productId)
    {
        await productRepository.TransferProduct(productId);
        var TotalDurationTime = MallDbInterceptor.Durations.Aggregate((z, e) => z + e);
        Console.WriteLine("Total duration time: " +
                          TotalDurationTime);
        MallDbInterceptor.Durations.Clear();
        return Ok(new
        {
            Monitoring = memoryCache.Get(InterceptorConsts.MonitoringCacheKey) ?? null,
            TotalDurationTime
        });
    }

    [HttpPost]
    public async Task<ActionResult<DbMonitoringDto>> AddProduct(Product product)
    {
        try
        {
            await productRepository.AddProduct(product);
            var TotalDurationTime = MallDbInterceptor.Durations.Aggregate((z, e) => z + e);
            Console.WriteLine("Total duration time: " +
                              TotalDurationTime);
            MallDbInterceptor.Durations.Clear();
            return Ok(
                new
                {
                    Monitoring = memoryCache.Get(InterceptorConsts.MonitoringCacheKey) ?? null,
                    TotalDurationTime
                });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<DbMonitoringDto>> UpdateProduct(Product product)
    {
        try
        {
            await productRepository.UpdateProduct(product);
            var TotalDurationTime = MallDbInterceptor.Durations.Aggregate((z, e) => z + e);
            Console.WriteLine("Total duration time: " +
                              TotalDurationTime);
            MallDbInterceptor.Durations.Clear();
            return Ok(
                new
                {
                    Monitoring = memoryCache.Get(InterceptorConsts.MonitoringCacheKey) ?? null,
                    TotalDurationTime
                });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
}