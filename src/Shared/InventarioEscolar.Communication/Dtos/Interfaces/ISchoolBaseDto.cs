namespace InventarioEscolar.Communication.Dtos.Interfaces
{
    public interface ISchoolBaseDto
    {
        public string Name { get; set; } 
        public string? Inep { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
    }
}
