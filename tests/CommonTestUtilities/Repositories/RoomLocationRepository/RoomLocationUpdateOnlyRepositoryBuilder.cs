using InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations;
using NSubstitute;

namespace CommonTestUtilities.Repositories.RoomLocationRepository
{
    public class RoomLocationUpdateOnlyRepositoryBuilder
    {
        private readonly IRoomLocationUpdateOnlyRepository _repository;
        public RoomLocationUpdateOnlyRepositoryBuilder()
        {
            _repository = Substitute.For<IRoomLocationUpdateOnlyRepository>();
        }
        public IRoomLocationUpdateOnlyRepository Build() => _repository;
    }
}