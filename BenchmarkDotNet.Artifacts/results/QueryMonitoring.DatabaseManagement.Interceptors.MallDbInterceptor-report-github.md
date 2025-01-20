```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4751/23H2/2023Update/SunValley3)
Intel Core i7-8565U CPU 1.80GHz (Whiskey Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.403
  [Host] : .NET 8.0.12 (8.0.1224.60305), X64 RyuJIT AVX2 DEBUG


```
| Mean | StdErr | StdDev | Min | Max | Q1 | Q3 | Median | Op/s | Rank |
|-----:|-------:|-------:|----:|----:|---:|---:|-------:|-----:|-----:|
|   NA |     NA |     NA |  NA |  NA | NA | NA |     NA |   NA |    ? |

Benchmarks with issues:
  MallDbInterceptor.ReaderExecutedAsync: Job-ZPUMNC(OutlierMode=DontRemove, Server=True, RunStrategy=Monitoring) [command=DbCommand, eventData=CommandExecutedEventData, result=DbDataReader, cancellationToken=CancellationToken]
