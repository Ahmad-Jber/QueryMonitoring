using System.Data;
using System.Data.Common;
using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.dotMemory;
using BenchmarkDotNet.Diagnostics.dotTrace;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Running;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using Perfolizer.Mathematics.OutlierDetection;
using QueryMonitoring.Consts;
using QueryMonitoring.Dto;

namespace QueryMonitoring.DatabaseManagement.Interceptors;

[Outliers(OutlierMode.DontRemove)]
[SimpleJob(RunStrategy.Monitoring)]
[MemoryDiagnoser]
[
    RankColumn,
    MinColumn,
    MaxColumn,
    Q1Column,
    Q3Column,
    AllStatisticsColumn,
]
[JsonExporterAttribute.Full, CsvMeasurementsExporter, CsvExporter(CsvSeparator.Comma), HtmlExporter,
 MarkdownExporterAttribute.GitHub]
[GcServer(true)]
[JsonExporterAttribute.Full()]
[DotMemoryDiagnoser]
[DotTraceDiagnoser]
[RPlotExporter]
[ReturnValueValidator(true)]
public class MallDbInterceptor : DbCommandInterceptor
{
    public static List<TimeSpan> Durations;
    private static string _description;
    private readonly IMemoryCache _memoryCache;

    public MallDbInterceptor(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        Durations = new List<TimeSpan>();
    }

    [Benchmark]
    public override ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData,
        DbDataReader result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        Console.WriteLine(_description);
        Console.WriteLine($"Database: {command.Connection?.Database}");
        Console.WriteLine($"Connection Status: {command.Connection?.State}");
        Console.WriteLine($"Command Type: {command.CommandType}");
        Console.WriteLine($"Timeout: {command.CommandTimeout}");
        Console.WriteLine($"Duration: {eventData.Duration}");
        var monitoringDto = new DbMonitoringDto
        {
            DatabaseName = command.Connection?.Database,
            Duration = eventData.Duration,
            Query = command.CommandText,
        };
        FetchServerMetrics(command.Connection!, monitoringDto);
        _memoryCache.Set(InterceptorConsts.MonitoringCacheKey, monitoringDto);
        Durations.Add(eventData.Duration);
        return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
    }

    private void FetchServerMetrics(DbConnection connection, DbMonitoringDto dbMonitoringDto)
    {
        try
        {
            // Ensure the connection is open
            if (connection.State != ConnectionState.Open)
                connection.Open();

            using var metricsCommand = connection.CreateCommand();
            metricsCommand.CommandText =
                @"
SELECT 
    (SELECT SUM(total_worker_time) FROM sys.dm_exec_query_stats) AS TotalCpuTime, 
    (SELECT cntr_value / 1024 AS MemoryInUseMB 
     FROM sys.dm_os_performance_counters 
     WHERE counter_name = 'Total Server Memory (KB)') AS MemoryInUseMB;
";

            using var reader = metricsCommand.ExecuteReader();
            if (reader.Read())
            {
                dbMonitoringDto.TotalCpuTime = new TimeSpan(reader.GetInt64(0));
                dbMonitoringDto.TotalMemoryInUse = reader.GetInt64(1) / 1024.0;
                Console.WriteLine($"Database CPU Time: {dbMonitoringDto.TotalCpuTime} ms");
                Console.WriteLine($"Database Memory Usage: {dbMonitoringDto.TotalMemoryInUse} MB");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching server metrics: {ex.Message}");
        }
    }
}