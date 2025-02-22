namespace InventarioEscolar.Domain.Repositories
{
    public interface IUnitOfWork
    {
        public Task Commit();
    }
}
