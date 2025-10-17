using InventarioEscolar.Communication.Dtos.Interfaces;

namespace InventarioEscolar.Communication.Dtos
{
    public record AssetMovementDto : IAssetMovementBaseDto
    {
        public long Id { get; set; }

        public long AssetId { get; set; }
        public long FromRoomId { get; set; }
        public long ToRoomId { get; set; }

        public AssetDto? Asset { get; set; }
        public RoomLocationDto? FromRoom { get; set; }
        public RoomLocationDto? ToRoom { get; set; }

        public DateTime MovedAt { get; set; } = DateTime.UtcNow;
        public string? Responsible { get; set; }
        public bool IsCanceled { get; set; }
        public string? CancelReason { get; set; }
        public DateTime? CanceledAt { get; set; }
    }
}