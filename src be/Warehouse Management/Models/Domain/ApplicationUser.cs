using Microsoft.AspNetCore.Identity;

namespace Warehouse_Management.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public bool Status { get; set; } = true;
    }
}
