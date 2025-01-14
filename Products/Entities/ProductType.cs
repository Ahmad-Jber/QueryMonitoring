using QueryMonitoring.Enums;

namespace QueryMonitoring.Products.Entities;

public class ProductType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public PriceCategoryEnum PriceCategory { get; set; }
}