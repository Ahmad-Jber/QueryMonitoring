using System.ComponentModel.DataAnnotations.Schema;
using QueryMonitoring.Enums;
using QueryMonitoring.Shops;

namespace QueryMonitoring.Products.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
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
