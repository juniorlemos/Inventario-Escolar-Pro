using InventarioEscolar.Communication.Enum;
using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Application.Dtos
{
    public record AssetDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long? PatrimonyCode { get; set; }
        public decimal? AcquisitionValue { get; set; }
        public ConservationState ConservationState { get; set; }
        public string? SerieNumber { get; set; }
        public required CategoryDto Category { get; set; }
        public RoomLocationDto? RoomLocation { get; set; }
    }
}
