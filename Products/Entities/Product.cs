using System.ComponentModel.DataAnnotations.Schema;
using QueryMonitoring.Entities;
using QueryMonitoring.Enums;
using QueryMonitoring.Shops;

namespace QueryMonitoring.Products.Entities
{
    public class Product: EntityBase<Guid>
    {
        public Product()
        {
            Id = Guid.NewGuid();
        }

        public Product(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Price = product.Price;
            ProductTypeId = product.ProductTypeId;
            Manufacturer = product.Manufacturer;
            Size = product.Size;
            Quantity = product.Quantity;
            Quality = product.Quality;
            ShopId = product.ShopId;
        }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public int ProductTypeId { get; set; }
        [ForeignKey("ProductTypeId")]
        public ProductType ProductType { get; set; }
        public int? Quantity { get; set; }
        public float Size { get; set; }
        public string Manufacturer { get; set; }
        public QualityEnum Quality { get; set; }
        public Guid ShopId { get; set; }
        [ForeignKey("ShopId")]
        public Shop Shop { get; set; }
    }
}
