namespace InventarioEscolar.Communication.Request
{
    public record RequestRegisterRoomLocationJson
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Building { get; set; }
        public long SchoolId { get; set; }

    }
}
