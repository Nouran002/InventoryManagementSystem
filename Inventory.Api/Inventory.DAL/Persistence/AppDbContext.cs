using Inventory.BL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Inventory.DAL.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

       
        public DbSet<Product> Products { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseProduct> warehouseProducts { get; set; }
        public DbSet<InventoryTransaction> inventoryTransactions { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure composite key for Inventory
            modelBuilder.Entity<WarehouseProduct>()
                .HasKey(i => new { i.ProductId, i.WarehouseId });

            // Configure relationships for Inventory
            modelBuilder.Entity<WarehouseProduct>()
                .HasOne(i => i.Product)
                .WithMany(p => p.WarehouseProducts)
                .HasForeignKey(i => i.ProductId);

            modelBuilder.Entity<WarehouseProduct>()
                .HasOne(i => i.Warehouse)
                .WithMany(w => w.WarehouseProducts)
                .HasForeignKey(i => i.WarehouseId);

            // Configure relationships for Transaction
            modelBuilder.Entity<InventoryTransaction>()
                .HasOne(t => t.Product)
                .WithMany(p => p.Transactions)
                .HasForeignKey(t => t.ProductId);

            modelBuilder.Entity<InventoryTransaction>()
                .HasOne(t => t.Warehouse)
                .WithMany(w => w.inventoryTransactions)
                .HasForeignKey(t => t.WarehouseId);

            modelBuilder.Entity<InventoryTransaction>()
                .HasOne(t => t.User)
                .WithMany(u => u.InventoryTransactions)
                .HasForeignKey(t => t.UserId);





        }
    }
}
