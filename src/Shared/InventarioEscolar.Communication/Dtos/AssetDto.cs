using InventarioEscolar.Communication.Dtos.Interfaces;
using InventarioEscolar.Domain.Enums;

namespace InventarioEscolar.Communication.Dtos
{
    public record AssetDto : IAssetBaseDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long? PatrimonyCode { get; set; }
        public decimal? AcquisitionValue { get; set; }
        public ConservationState ConservationState { get; set; }
        public string? SerieNumber { get; set; }
        public long CategoryId { get; set; }
        public CategoryDto Category { get; set; }
        public long? RoomLocationId { get; set; }
        public RoomLocationDto? RoomLocation { get; set; }
        public long SchoolId { get; set; }
    }
}