using Microsoft.EntityFrameworkCore;
using Warehouse_Management.Data;
using Warehouse_Management.Models.Domain;

namespace Warehouse_Management.Seeder
{
    public class DbSeeder
    {
        public static void SeedData(WareHouseDbContext context, ILogger logger)
        {
            try
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();  // Apply migration mới
                }

                if (!context.Products.Any())
                {
                    var products = new[]
                    {
                        new Product
                        {
                            SKU = "P001",
                            Barcode = "1234567890",
                            ProductName = "Product 1",
                            Description = "Description of Product 1",
                            BasePrice = 100,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        }
                    };

                    // Thêm các sản phẩm vào cơ sở dữ liệu
                    context.Products.AddRange(products);
                    context.SaveChanges();
                    logger.LogInformation("1 products have been seeded into the database.");
                }
                else
                {
                    logger.LogInformation("Products table already contains data, no seed required.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while seeding the database: {ex.Message}");
            }
        }
    }
}
