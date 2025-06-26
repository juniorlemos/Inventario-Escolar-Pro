namespace InventarioEscolar.Communication.Dtos
{
    public record RoomLocationSummaryDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
