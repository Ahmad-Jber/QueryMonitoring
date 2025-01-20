using Microsoft.EntityFrameworkCore;
using QueryMonitoring.DatabaseManagement.DbContexts;
using QueryMonitoring.DatabaseManagement.Interceptors;
using QueryMonitoring.DatabaseManagement.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Register HttpClient (using IHttpClientFactory is the preferred method)
builder.Services.AddHttpClient();  // Register IHttpClientFactory
builder.Services.AddMemoryCache();
// Register your other services (DbContexts, Repositories, etc.)
builder.Services.AddScoped<MallDbInterceptor>();
builder.Services.AddDbContext<AlQimaDbContext>((serviceProvider, options) =>
{
    var interceptor = serviceProvider.GetRequiredService<MallDbInterceptor>();
    options.UseSqlServer(builder.Configuration.GetConnectionString("AlQimaDb"))
        .AddInterceptors(interceptor);
});
builder.Services.AddDbContext<RamallahDbContext>((serviceProvider, options) =>
{
    var interceptor = serviceProvider.GetRequiredService<MallDbInterceptor>();
    options.UseSqlServer(builder.Configuration.GetConnectionString("RamallahMall"))
        .AddInterceptors(interceptor);
});
builder.Services.AddTransient<IProductRepository, ProductRepository>();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRazorPages();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", r =>
    {
        r.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapRazorPages();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();
