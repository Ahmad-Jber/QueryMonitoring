using Microsoft.EntityFrameworkCore;
using QueryMonitoring.DatabaseManagement.DbContexts;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AlQimaDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("AlQimaDb")));
builder.Services.AddDbContext<BrandDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("BrandDb")));
builder.Services.AddDbContext<RamallahDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("RamallahMall")));
builder.Services.AddDbContext<LacasaDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("LacasaMall")));
builder.Services.AddDbContext<CityDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("CityMall")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();