
namespace QueryMonitoring.Dto;

public class DbMonitoringDto
{
    public string DatabaseName { get; set; }
    public string Query { get; set; }
    public TimeSpan Duration { get; set; }
    public TimeSpan TotalCpuTime { get; set; }

    public double TotalMemoryInUse { get; set; }
}