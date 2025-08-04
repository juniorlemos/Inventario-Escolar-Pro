using CommonTestUtilities.Repositories.AssetRepository;
using CommonTestUtilities.Repositories.RoomLocationRepository;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Domain.Pagination;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTestUtilities.Repositories.SchoolRepository
{
    public class SchoolReadOnlyRepositoryBuilder
    {
        private readonly ISchoolReadOnlyRepository _repository;
        public SchoolReadOnlyRepositoryBuilder()
        {
            _repository = Substitute.For<ISchoolReadOnlyRepository>();
        }
        public SchoolReadOnlyRepositoryBuilder WithSchoolExist(long id, School school)
        {
            _repository.GetById(id).Returns(school);
            return this;
        }
        public SchoolReadOnlyRepositoryBuilder WithSchoolNotFound(long id)
        {
            _repository.GetById(id).Returns((School)null);
            return this;
        }
        public SchoolReadOnlyRepositoryBuilder WithDuplicateSchool(School? school)
        {
            _repository
                .GetDuplicateSchool(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(school);

            return this;
        }
        public SchoolReadOnlyRepositoryBuilder WithSchoolsExist(IEnumerable<School> schools, int page = 1, int pageSize = 10)
        {
            var schoolList = schools.ToList();
            var skip = (page - 1) * pageSize;
            var pagedItems = schoolList.Skip(skip).Take(pageSize).ToList();

            var pagedResult = new PagedResult<School>(
                pagedItems,
                schoolList.Count,
                page,
                pageSize
            );

            _repository.GetAll(page, pageSize).Returns(pagedResult);

            return this;
        }

        public SchoolReadOnlyRepositoryBuilder WithGetAllReturningNull(int page, int pageSize)
        {
            _repository.GetAll(page, pageSize).Returns((PagedResult<School>?)null);
            return this;
        }
        public ISchoolReadOnlyRepository Build() => _repository;
    }
}
