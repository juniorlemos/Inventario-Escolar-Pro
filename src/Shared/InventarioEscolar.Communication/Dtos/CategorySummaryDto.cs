namespace InventarioEscolar.Communication.Dtos
{
    public record CategorySummaryDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
