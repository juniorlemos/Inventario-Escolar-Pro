namespace InventarioEscolar.Communication.Response
{
    public record RoomLocationListDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Building { get; set; }
        public long SchoolId { get; set; }
    }
}
