using CommonTestUtilities.Repositories.CategoryRepository;
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
    public class RoomLocationDeleteOnlyRepositoryBuilder
    {
        private readonly IRoomLocationDeleteOnlyRepository _repository;
        public RoomLocationDeleteOnlyRepositoryBuilder()
        {
            _repository = Substitute.For<IRoomLocationDeleteOnlyRepository>();
        }

        public RoomLocationDeleteOnlyRepositoryBuilder WithDeleteReturningTrue(long roomLocationId)
        {
            _repository.Delete(roomLocationId).Returns(true);
            return this;
        }
        public IRoomLocationDeleteOnlyRepository Build() => _repository;

    }
}

