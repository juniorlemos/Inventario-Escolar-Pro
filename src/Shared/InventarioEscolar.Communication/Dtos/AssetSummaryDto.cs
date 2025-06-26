namespace InventarioEscolar.Communication.Dtos
{
    public record AssetSummaryDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
