using QueryMonitoring.Entities;
using QueryMonitoring.Products.Entities;

namespace QueryMonitoring.Shops;

public class Shop: EntityBase<Guid>
{
    public Shop()
    {
        Id = Guid.NewGuid();
    }
    public string Name;
    public string Address;
    public ICollection<ProductType> ProductTypes;
}