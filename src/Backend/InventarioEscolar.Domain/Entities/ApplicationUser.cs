
using Microsoft.AspNetCore.Identity;

namespace InventarioEscolar.Domain.Entities
{
    public class ApplicationUser : IdentityUser<long>
    {
        public long SchoolId { get; set; }
        public virtual School School { get; set; } = null!;
    }
}