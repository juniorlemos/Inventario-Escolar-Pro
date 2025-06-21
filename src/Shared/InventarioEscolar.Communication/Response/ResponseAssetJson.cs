using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Communication.Enum;

namespace InventarioEscolar.Communication.Response
{
    public record ResponseAssetJson
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long? PatrimonyCode { get; set; }
        public decimal? AcquisitionValue { get; set; }
        public ConservationState ConservationState { get; set; }
        public string? SerieNumber { get; set; }
        public CategoryDto Category { get; set; } = null!;
        public RoomLocationDto? RoomLocation { get; set; }
    }
}
