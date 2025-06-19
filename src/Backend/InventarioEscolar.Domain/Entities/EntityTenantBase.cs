namespace InventarioEscolar.Domain.Entities
{
    public class EntityTenantBase : EntityBase
    {
        public long SchoolId { get; set; }
        public School School { get; set; } = null!;
    }
}
