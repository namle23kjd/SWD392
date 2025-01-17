using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Warehouse_Management.Data
{
    public class WareHouseDbContext : IdentityDbContext
    {
        public WareHouseDbContext(DbContextOptions<WareHouseDbContext> options) : base(options)
        {
        }

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
        }
    }
}
