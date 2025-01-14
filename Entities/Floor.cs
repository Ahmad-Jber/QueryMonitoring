using QueryMonitoring.Shops;

namespace QueryMonitoring.Entities;

public class Floor
{
    public int Id { get; set; }
    public int Position { get; set; }
    public ICollection<Shop> Shops { get; set; } = new List<Shop>();
}