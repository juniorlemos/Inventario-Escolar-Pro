namespace InventarioEscolar.Domain.Entities
{
    public class Category :EntityBase
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public IList<Asset> Assets { get; set; } = [];
    }
}
