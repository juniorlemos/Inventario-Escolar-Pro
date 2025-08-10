namespace InventarioEscolar.Domain.Entities
{
    public class School : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string? Inep { get; set; } 
    public string? Address { get; set; }
    public string? City { get; set; }
    public ICollection<RoomLocation> RoomLocations { get; set; } = [];
    public ICollection<Asset> Assets { get; set; } = [];
    public ICollection<Category> Categories { get; set; } = [];
    public ICollection<ApplicationUser> Users { get; set; } = [];
    }
}