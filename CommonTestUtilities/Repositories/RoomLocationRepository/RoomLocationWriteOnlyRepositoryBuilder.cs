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

