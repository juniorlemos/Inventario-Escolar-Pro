namespace InventarioEscolar.Domain.Entities;
public class EntityBase
{
    public long Id { get; private set; }
    public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;
    public bool Active { get; private set; } = true;
}
