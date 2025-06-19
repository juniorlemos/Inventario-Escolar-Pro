namespace InventarioEscolar.Application.Dtos
{
    public record CategoryDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long SchoolId { get; set; }
    }
}
