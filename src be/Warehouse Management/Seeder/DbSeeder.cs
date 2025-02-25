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
                context.Database.Migrate();

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
                        },
                        new Product
                        {
                            SKU = "P002",
                            Barcode = "1234567891",
                            ProductName = "Product 2",
                            Description = "Description of Product 2",
                            BasePrice = 150,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        },
                        new Product
                        {
                            SKU = "P003",
                            Barcode = "1234567892",
                            ProductName = "Product 3",
                            Description = "Description of Product 3",
                            BasePrice = 200,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        },
                        new Product
                        {
                            SKU = "P004",
                            Barcode = "1234567893",
                            ProductName = "Product 4",
                            Description = "Description of Product 4",
                            BasePrice = 250,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        }
                    };

                    // Thêm các sản phẩm vào cơ sở dữ liệu
                    context.Products.AddRange(products);
                    context.SaveChanges();
                    logger.LogInformation("4 products have been seeded into the database.");
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
