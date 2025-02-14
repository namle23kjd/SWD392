using System.ComponentModel.DataAnnotations;

namespace Warehouse_Management.Models.Domain
{
    public class Platform
    {
        [Key]
        public int PlatformId { get; set; }
        public string? Name { get; set; }
        public string? ApiKey { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}
