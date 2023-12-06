using Entities.Configuration;
using Entities.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Data
{
    public class ResDbContext : IdentityDbContext<User>
    {
        public ResDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasMany(p => p.Products).WithOne(c => c.Category).HasForeignKey(c=>c.CategoryId);
            modelBuilder.Entity<Product>().HasMany(p => p.Images).WithOne(p => p.Product).HasForeignKey(c => c.ProductId);
            modelBuilder.Entity<CartItem>().HasKey(ci => new { ci.UserId, ci.ProductId });
            modelBuilder.Entity<CartItem>().HasOne(c => c.User).WithMany(u => u.CartItems).HasForeignKey(c => c.UserId);
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImages> Images { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CartItem> CartItem { get; set; }
    }
}
