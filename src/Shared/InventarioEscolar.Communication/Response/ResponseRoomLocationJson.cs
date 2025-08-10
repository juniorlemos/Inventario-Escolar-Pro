using InventarioEscolar.Communication.Dtos;

namespace InventarioEscolar.Communication.Response
{
    public record ResponseRoomLocationJson
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Building { get; set; }
        public ICollection<AssetSummaryDto> Assets { get; set; } = [];
    }
}