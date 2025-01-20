using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse_Management.Models.Domain
{
    public class Shelf
    {
        [Key]
        public int ShelfId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public int Capacity { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public ICollection<Lot>? Lots { get; set; }
    }
}
