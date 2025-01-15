using QueryMonitoring.Products.Entities;

namespace QueryMonitoring.Shops;

public class Shop
{
    public Shop()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }
    public string Name;
    public string Address;
    public ICollection<ProductType> ProductTypes;
}