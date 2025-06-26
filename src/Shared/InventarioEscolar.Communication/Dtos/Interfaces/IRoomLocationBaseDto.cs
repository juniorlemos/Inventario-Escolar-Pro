namespace InventarioEscolar.Communication.Dtos.Interfaces
{
    public interface IRoomLocationBaseDto
    {
        string Name { get; set; }
        string? Description { get; set; }
        string? Building { get; set; }
    }
}
