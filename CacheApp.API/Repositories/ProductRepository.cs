using CacheApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CacheApp.API.Repositories;

internal class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
        _context.Database.EnsureCreated();
    }

    public async Task<List<Product>> Get()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> Get(int id)
    {
        return await _context.Products.FindAsync(id);
    }
}