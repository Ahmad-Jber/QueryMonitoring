using Microsoft.EntityFrameworkCore;

namespace QueryMonitoring.Entities;

public class Attachment
{
    public Guid Id { get; set; }
    public string EntityId { get; set; }
    public string EntityName { get; set; }
    public string Name { get; set; }
    public string ContentType { get; set; }
    public byte[] Content { get; set; }
}