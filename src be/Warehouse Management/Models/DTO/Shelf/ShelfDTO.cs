namespace Warehouse_Management.Models.DTO.Shelf
{
    public class ShelfDTO
    {
        public int ShelfId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public int Capacity { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? UserName { get; set; }
    }
}
