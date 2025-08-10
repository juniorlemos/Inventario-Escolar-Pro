using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using NSubstitute;

namespace CommonTestUtilities.Repositories.SchoolRepository
{
    public class SchoolUpdateOnlyRepositoryBuilder
    {
        private readonly ISchoolUpdateOnlyRepository _repository;
        public SchoolUpdateOnlyRepositoryBuilder()
        {
            _repository = Substitute.For<ISchoolUpdateOnlyRepository>();
        }
        public ISchoolUpdateOnlyRepository Build() => _repository;
    }
}