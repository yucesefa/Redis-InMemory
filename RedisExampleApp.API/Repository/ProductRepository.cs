﻿using Microsoft.EntityFrameworkCore;
using RedisExampleApp.API.Models;

namespace RedisExampleApp.API.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            _context.Products.AddAsync(product);
            _context.SaveChangesAsync();
            return product;
        }

        public async Task<List<Product>> GetAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
           return await _context.Products.FindAsync(id);
        }
    }
}
