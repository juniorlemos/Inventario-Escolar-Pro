using InventarioEscolar.Domain.Interfaces.Repositories.Categories;
using InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
