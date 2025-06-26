namespace InventarioEscolar.Domain.Entities;
public abstract class EntityBase
{
    public long Id { get; set; }
    public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;
    public bool Active { get; set; } = true;
}
