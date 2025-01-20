using QueryMonitoring.Enums;

namespace QueryMonitoring.Entities;

public class EntityBase<TPrimaryKey>
{
    public TPrimaryKey? Id { get; set; }
    public DatabaseEnum Database { get; set; }
}