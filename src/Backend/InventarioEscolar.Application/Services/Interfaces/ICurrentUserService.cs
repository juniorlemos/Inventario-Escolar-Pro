namespace InventarioEscolar.Application.Services.Interfaces
{
    public interface ICurrentUserService
    {
        long SchoolId { get; }
        long? UserId { get; }
        string? UserName { get; }
        bool IsAuthenticated { get; }
    }
}