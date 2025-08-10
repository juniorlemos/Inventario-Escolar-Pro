using InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations;
using NSubstitute;

namespace CommonTestUtilities.Repositories.RoomLocationRepository
{
    public class RoomLocationWriteOnlyRepositoryBuilder
    {
    private readonly IRoomLocationWriteOnlyRepository _repository;
    public RoomLocationWriteOnlyRepositoryBuilder()
    {
        _repository = Substitute.For<IRoomLocationWriteOnlyRepository>();
    }
    public IRoomLocationWriteOnlyRepository Build() => _repository;
    }
}
