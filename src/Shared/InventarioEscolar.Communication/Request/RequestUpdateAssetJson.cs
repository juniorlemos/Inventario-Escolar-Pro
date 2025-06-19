using InventarioEscolar.Communication.Enum;

namespace InventarioEscolar.Communication.Request
{
    public record RequestUpdateAssetJson
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long? PatrimonyCode { get; set; }
        public decimal? AcquisitionValue { get; set; }
        public ConservationState ConservationState { get; set; }
        public string? SerieNumber { get; set; }
        public long CategoryId { get; set; }
        public long? RoomLocationId { get; set; }
    }
}
