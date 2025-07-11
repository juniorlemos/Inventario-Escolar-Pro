using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Communication.Dtos;

namespace InventarioEscolar.Communication.Response
{
    public record ResponseAssetMovementJson
    {
        public long Id { get; set; }
        public AssetSummaryDto Asset { get; set; }
        public RoomLocationSummaryDto FromRoom { get; set; }
        public RoomLocationSummaryDto ToRoom { get; set; }
        public DateTime MovedAt { get; set; } = DateTime.UtcNow;
        public string? Responsible { get; set; }
        public bool IsCanceled { get; set; }
        public string? CancelReason { get; set; }
        public DateTime? CanceledAt { get; set; }
    }
}
