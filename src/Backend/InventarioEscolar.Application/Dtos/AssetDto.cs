using InventarioEscolar.Communication.Enum;
using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Application.Dtos
{
    public record AssetDto
    {
        public long Id { get; set; }
        public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long? PatrimonyCode { get; set; }
        public decimal? AcquisitionValue { get; set; }
        public ConservationState ConservationState { get; set; }
        public string? SerieNumber { get; set; }
        public required Category Category { get; set; }
        public RoomLocation? RoomLocation { get; set; }
    }
}
