using InventarioEscolar.Domain.Interfaces;

namespace InventarioEscolar.Domain.Entities
{
    public class EntityTenantBase : EntityBase , ISchoolEntity
    {
        public long SchoolId { get; set; }
        public School School { get; set; } = null!;
    }
}