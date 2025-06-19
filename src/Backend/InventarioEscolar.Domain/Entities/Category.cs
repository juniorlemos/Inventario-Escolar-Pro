namespace InventarioEscolar.Domain.Entities
{
    public class Category : EntityTenantBase
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public ICollection<Asset> Assets { get; set; } = [];
    }
}
