namespace InventarioEscolar.Communication.Dtos
{
    public record RoomLocationDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Building { get; set; }
        public long SchoolId { get; set; }

    }
}
