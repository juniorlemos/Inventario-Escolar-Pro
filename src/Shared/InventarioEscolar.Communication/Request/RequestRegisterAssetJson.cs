using InventarioEscolar.Communication.Enum;

namespace InventarioEscolar.Communication.Request
{
    public record RequestRegisterAssetJson
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long? PatrimonyCode { get; set; }
        public decimal? AcquisitionValue { get; set; }
        public ConservationState ConservationState { get; set; }
        public string? SerieNumber { get; set; }
        public long CategoryId { get; set; } 
        public long? RoomLocationId { get; set; }
        public long SchoolId { get; set; }
    }
}
