namespace InventarioEscolar.Communication.Request
{
    public record RequestRegisterAssetMovementJson
    {
        public long AssetId { get; set; }
        public long FromRoomId { get; set; }
        public long ToRoomId { get; set; }
        public string? Responsible { get; set; }
    }
}
