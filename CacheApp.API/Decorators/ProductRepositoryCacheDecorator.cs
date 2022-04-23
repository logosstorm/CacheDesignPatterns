using System.Text.Json;
using CacheApp.API.Models;
using CacheApp.API.Repositories;
using CacheApp.Cache;
using StackExchange.Redis;

namespace CacheApp.API.Decorators;

public class ProductRepositoryCacheDecorator : IProductRepository
{
    private const string ProductKey = "products";
    private readonly IDatabase _cacheDatabase;
    private readonly IProductRepository _repository;

    public ProductRepositoryCacheDecorator(IProductRepository repository, RedisService redisService)
    {
        _repository = repository;

        _cacheDatabase = redisService.GetDb(0);
    }

    public async Task<List<Product>> Get()
    {
        List<Product> products = new();

        if (_cacheDatabase.KeyExists(ProductKey))
        {
            foreach (var item in (await _cacheDatabase.HashGetAllAsync(ProductKey)).ToList())
            {
                var product = JsonSerializer.Deserialize<Product>(item.Value);
                products.Add(product!);
            }

            return products;
        }

        return await LoadCacheFromDb();
    }

    public async Task<Product?> Get(int id)
    {
        if (_cacheDatabase.KeyExists(ProductKey))
        {
            var product = await _cacheDatabase.HashGetAsync(ProductKey, id);
            return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : null;
        }

        return (await LoadCacheFromDb()).FirstOrDefault(x => x.Id == id);
    }

    private async Task<List<Product>> LoadCacheFromDb()
    {
        var products = await _repository.Get();
        products.ForEach(p => { _cacheDatabase.HashSetAsync(ProductKey, p.Id, JsonSerializer.Serialize(p)); });

        return products;
    }
}