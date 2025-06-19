namespace InventarioEscolar.Domain.Entities
{
    public class RoomLocation : EntityTenantBase
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Building { get; set; }

        public ICollection<Asset> Assets { get; set; } = [];
    }
}
