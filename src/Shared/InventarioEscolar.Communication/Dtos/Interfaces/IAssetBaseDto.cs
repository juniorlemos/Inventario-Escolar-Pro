using InventarioEscolar.Communication.Enum;

namespace InventarioEscolar.Communication.Dtos.Interfaces
{
    public interface IAssetBaseDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public long? PatrimonyCode { get; set; }
        public decimal? AcquisitionValue { get; set; }
        public ConservationState ConservationState { get; set; }
        public string? SerieNumber { get; set; }
    }
}
