using InventarioEscolar.Communication.Dtos.Interfaces;
using InventarioEscolar.Domain.Enums;

namespace InventarioEscolar.Communication.Dtos
{
    public record UpdateAssetDto : IAssetBaseDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public long? PatrimonyCode { get; set; }
        public decimal? AcquisitionValue { get; set; }
        public ConservationState ConservationState { get; set; }
        public string? SerieNumber { get; set; }
        public long CategoryId { get; set; }
        public long RoomLocationId { get; set; }
    }
}