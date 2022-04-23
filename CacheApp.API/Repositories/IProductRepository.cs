using CacheApp.API.Models;

namespace CacheApp.API.Repositories;

public interface IProductRepository
{
    Task<List<Product>> Get();

    Task<Product?> Get(int id);
}