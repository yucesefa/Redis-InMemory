﻿using Microsoft.EntityFrameworkCore;

namespace RedisExampleApp.API.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasData(
                new Product()
                {
                    Id = 1,
                    Name = "Kalem 1",
                    Price = 123
                },
                new Product()
                {
                    Id = 2,
                    Name = "Kalem 2",
                    Price = 1234
                },
                new Product()
                {
                    Id = 3,
                    Name = "Kalem 3",
                    Price = 1235
                }
                );
        }
    }


}
