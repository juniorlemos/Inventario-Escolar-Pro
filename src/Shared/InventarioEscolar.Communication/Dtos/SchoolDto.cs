namespace InventarioEscolar.Communication.Dtos
{
    public record SchoolDto
    {
        public long Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string? Inep { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
    }
}
