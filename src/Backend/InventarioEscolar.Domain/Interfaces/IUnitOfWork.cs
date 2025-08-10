namespace InventarioEscolar.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task Commit();
        Task ExecuteInTransaction(Func<Task> operation);
    }
}