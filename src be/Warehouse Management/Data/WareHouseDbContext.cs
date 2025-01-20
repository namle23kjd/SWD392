using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Warehouse_Management.Models.Domain;

namespace Warehouse_Management.Data
{
    public class WareHouseDbContext : IdentityDbContext<ApplicationUser>
    {
        public WareHouseDbContext(DbContextOptions<WareHouseDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<StockTransaction> StockTransactions { get; set; }
        public DbSet<Shelf> Shelves { get; set; }
        public DbSet<Lot> Lots { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Platform> Platforms { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var adminID = "373e112e - 121b - 4f3f - bb3e - dc30c08b9999";
            var staffID = "9116bb38-85b1-4345-bf4a-7c0819a8ef3b";

            var role = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = adminID,
                    ConcurrencyStamp = adminID,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper()
                },
                new IdentityRole
                {
                    Id = staffID,
                    ConcurrencyStamp = staffID,
                    Name = "Staff",
                    NormalizedName = "Staff".ToUpper()
                }
            };
            

            builder.Entity<IdentityRole>().HasData(role);

            // Cascade Delete trên Product -> StockTransaction
            builder.Entity<StockTransaction>()
                .HasOne(st => st.Product)
                .WithMany(p => p.StockTransactions)
                .HasForeignKey(st => st.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Chỉ giữ Cascade Delete ở đây

            // Không Cascade Delete trên Supplier -> StockTransaction
            builder.Entity<StockTransaction>()
                .HasOne(st => st.Supplier)
                .WithMany(s => s.StockTransactions)
                .HasForeignKey(st => st.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            // Không Cascade Delete trên Lot -> StockTransaction
            builder.Entity<StockTransaction>()
                .HasOne(st => st.Lot)
                .WithMany(l => l.StockTransactions)
                .HasForeignKey(st => st.LotId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
