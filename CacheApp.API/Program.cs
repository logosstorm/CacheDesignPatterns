using CacheApp.API.Decorators;
using CacheApp.API.Models;
using CacheApp.API.Repositories;
using CacheApp.Cache;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//no cache Repository
builder.Services.AddScoped<IProductRepository>(sp =>
{
    var dbContext = sp.GetRequiredService<AppDbContext>();
    var redisService = sp.GetRequiredService<RedisService>();
    var repository = new ProductRepository(dbContext);
    return new ProductRepositoryCacheDecorator(repository, redisService);


});
builder.Services.AddSingleton<RedisService>();
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("db"));
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