using CommonTestUtilities.Repositories.CategoryRepository;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Categories;
using InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations;
using InventarioEscolar.Domain.Pagination;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTestUtilities.Repositories.RoomLocationRepository
{
    public class RoomLocationReadOnlyRepositoryBuilder
    {
        private readonly IRoomLocationReadOnlyRepository _repository;
        public RoomLocationReadOnlyRepositoryBuilder()
        {
            _repository = Substitute.For<IRoomLocationReadOnlyRepository>();
        }
        public RoomLocationReadOnlyRepositoryBuilder WithRoomLocationExist(long id, RoomLocation RoomLocation)
        {
            _repository.GetById(id).Returns(RoomLocation);
            return this;
        }
        public RoomLocationReadOnlyRepositoryBuilder WithRoomLocationNotFound(long id)
        {
            _repository.GetById(id).Returns((RoomLocation)null);
            return this;
        }
        public RoomLocationReadOnlyRepositoryBuilder WithRoomLocationsExist(IEnumerable<RoomLocation> roomLocations, int page = 1, int pageSize = 10)
        {
            var roomLocationList = roomLocations.ToList();
            var skip = (page - 1) * pageSize;
            var pagedItems = roomLocationList.Skip(skip).Take(pageSize).ToList();

            var pagedResult = new PagedResult<RoomLocation>(
                pagedItems,
                roomLocationList.Count,
                page,
                pageSize
            );

            _repository.GetAll(page, pageSize).Returns(pagedResult);

            return this;
        }
        public RoomLocationReadOnlyRepositoryBuilder WithGetAllReturningNull(int page, int pageSize)
        {
            _repository.GetAll(page, pageSize).Returns((PagedResult<RoomLocation>?)null);
            return this;
        }
        public RoomLocationReadOnlyRepositoryBuilder WithRoomLocationNameNotExists(string name, long schoolId)
        {
            _repository.ExistRoomLocationName(name, schoolId).Returns(false);
            return this;
        }
        public RoomLocationReadOnlyRepositoryBuilder WithRoomLocationNameExists(string name, long schoolId)
        {
            _repository.ExistRoomLocationName(name, schoolId).Returns(true);
            return this;
        }
        public IRoomLocationReadOnlyRepository Build() => _repository;
    }
}

