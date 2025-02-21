namespace Warehouse_Management.Models.DTO.Supplier
{
    public class SupplierDTO
    {
        public int SupplierId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? CreateAt { get; set; }

        public bool IsDeleted { get; set; } = false;

    }
}
