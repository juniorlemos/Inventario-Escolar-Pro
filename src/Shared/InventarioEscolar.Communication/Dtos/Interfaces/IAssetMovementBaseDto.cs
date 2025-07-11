namespace InventarioEscolar.Communication.Dtos.Interfaces
{
    public interface IAssetMovementBaseDto
    {
        public long AssetId { get; set; }
        public long FromRoomId { get; set; }
        public long ToRoomId { get; set; }
        public DateTime MovedAt { get; set; }
        public string? Responsible { get; set; }
        public bool IsCanceled { get; set; } 
        public string? CancelReason { get; set; }
        public DateTime? CanceledAt { get; set; }
    }
}
