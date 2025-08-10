namespace InventarioEscolar.Communication.Request
{
    public record RequestUpdateRoomLocationJson
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Building { get; set; }
    }
}