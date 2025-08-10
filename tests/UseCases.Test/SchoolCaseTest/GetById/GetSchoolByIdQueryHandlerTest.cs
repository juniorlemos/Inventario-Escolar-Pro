using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.SchoolRepository;
using InventarioEscolar.Application.UsesCases.SchoolCase.GetById;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.SchoolCaseTest.GetById
{
    public class GetSchoolByIdQueryHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldReturnSchoolDto_WhenSchoolExists()
        {
            var school = SchoolBuilder.Build();
            var query = new GetByIdSchoolQuery(school.Id);

            var repository = CreateSchoolReadOnlyRepository(school.Id, school);
            var handler = CreateHandler(repository);

            var result = await handler.Handle(query, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Id.ShouldBe(school.Id);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenSchoolDoesNotExist()
        {
            var schoolId = 999; 
            var query = new GetByIdSchoolQuery(schoolId);

            var repository = CreateSchoolReadOnlyRepository(schoolId, null);
            var handler = CreateHandler(repository);

            var exception = await Should.ThrowAsync<NotFoundException>(() =>
                handler.Handle(query, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.SCHOOL_NOT_FOUND);
        }

        private static ISchoolReadOnlyRepository CreateSchoolReadOnlyRepository(long schoolId, School? school)
        {
            var builder = new SchoolReadOnlyRepositoryBuilder();

            return school is not null
                ? builder.WithSchoolExist(schoolId, school).Build()
                : builder.WithSchoolNotFound(schoolId).Build();
        }

        private static GetByIdSchoolQueryHandler CreateHandler(ISchoolReadOnlyRepository repository)
        {
            return new GetByIdSchoolQueryHandler(repository);
        }
    }
}