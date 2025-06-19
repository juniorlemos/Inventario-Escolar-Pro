namespace InventarioEscolar.Domain.Entities
{
    public class AssetMovement : EntityBase
    {
        public long AssetId { get; set; }
        public Asset Asset { get; set; } = null!;

        public long FromRoomId { get; set; }
        public RoomLocation FromRoom { get; set; } = null!;

        public long ToRoomId { get; set; }
        public RoomLocation ToRoom { get; set; } = null!;

        public DateTime MovedAt { get; set; } = DateTime.UtcNow;
        public string? Responsible { get; set; }
    }
}
