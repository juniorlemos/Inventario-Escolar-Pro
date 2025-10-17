using InventarioEscolar.Communication.Dtos.Interfaces;

namespace InventarioEscolar.Communication.Dtos
{
    public record RoomLocationDto : IRoomLocationBaseDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Building { get; set; }
        public ICollection<AssetDto> Assets { get; set; } = [];
    }
}