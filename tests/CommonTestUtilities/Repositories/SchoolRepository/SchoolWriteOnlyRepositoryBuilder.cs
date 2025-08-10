using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using NSubstitute;

namespace CommonTestUtilities.Repositories.SchoolRepository
{
    public class SchoolWriteOnlyRepositoryBuilder
    {
        private readonly ISchoolWriteOnlyRepository _repository;
        public SchoolWriteOnlyRepositoryBuilder()
        {
            _repository = Substitute.For<ISchoolWriteOnlyRepository>();
        }
        public ISchoolWriteOnlyRepository Build() => _repository;
    }
}