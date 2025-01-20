using QueryMonitoring.Entities;
using QueryMonitoring.Enums;

namespace QueryMonitoring.Products.Entities;

public class ProductType: EntityBase<int>
{
    public string Name { get; set; }
    public PriceCategoryEnum PriceCategory { get; set; }
}