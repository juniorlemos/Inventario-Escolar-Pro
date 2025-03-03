namespace InventarioEscolar.Domain.Entities
{
    public class RoomLocation :EntityBase
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Building { get; set; }
        public IList<Asset> Assets { get; set; } = [];
    }
}
