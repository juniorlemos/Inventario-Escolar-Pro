namespace InventarioEscolar.Domain.Entities
{
    public class AssetMovement : EntityTenantBase
    {
        public long AssetId { get; set; }
        public required Asset Asset { get; set; } 
        public long FromRoomId { get; set; }
        public required RoomLocation FromRoom { get; set; } 
        public long ToRoomId { get; set; }
        public required RoomLocation ToRoom { get; set; } 
        public DateTime MovedAt { get; set; } = DateTime.UtcNow;
        public string? Responsible { get; set; }
        public bool IsCanceled { get; set; } = false;
        public string? CancelReason { get; set; }
        public DateTime? CanceledAt { get; set; }
    }
}