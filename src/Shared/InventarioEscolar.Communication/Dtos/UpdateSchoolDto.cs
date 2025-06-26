using InventarioEscolar.Communication.Dtos.Interfaces;

namespace InventarioEscolar.Communication.Dtos
{
    public record UpdateSchoolDto : ISchoolBaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Inep { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
    }
}
