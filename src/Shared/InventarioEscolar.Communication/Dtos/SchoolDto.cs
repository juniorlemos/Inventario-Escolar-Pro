using InventarioEscolar.Communication.Dtos.Interfaces;

namespace InventarioEscolar.Communication.Dtos
{
    public record SchoolDto : ISchoolBaseDto
    {
        public long Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string? Inep { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public ICollection<RoomLocationDto> RoomLocations { get; set; } = [];
        public ICollection<AssetDto> Assets { get; set; } = [];
        public string Email { get; set; } = string.Empty;
    }
}