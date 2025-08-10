using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.SchoolRepository;
using InventarioEscolar.Application.UsesCases.SchoolCase.GetAll;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using Shouldly;

namespace UseCases.Test.SchoolCaseTest.GetAll
{
    public class GetAllSchoolQueryHandlerTest
    {
        private const int DefaultPage = 1;
        private const int DefaultPageSize = 10;

        [Fact]
        public async Task Handle_ShouldReturnPagedResult_WhenSchoolsExist()
        {
            const int totalCount = 20;
            const int expectedItemCount = 10;

            var schools = SchoolBuilder.BuildList(totalCount);
            var query = new GetAllSchoolQuery(DefaultPage, DefaultPageSize);

            var repository = CreateSchoolRepository(schools, DefaultPage, DefaultPageSize);
            var handler = CreateHandler(repository);

            var result = await handler.Handle(query, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Items.Count.ShouldBe(expectedItemCount);
            result.TotalCount.ShouldBe(totalCount);
            result.Page.ShouldBe(DefaultPage);
            result.PageSize.ShouldBe(DefaultPageSize);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyPagedResult_WhenNoSchoolsExist()
        {
            var query = new GetAllSchoolQuery(DefaultPage, DefaultPageSize);
            IList<School>? schools = null;

            var repository = CreateSchoolRepository(schools, DefaultPage, DefaultPageSize);
            var handler = CreateHandler(repository);

            var result = await handler.Handle(query, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Items.ShouldBeEmpty();
            result.TotalCount.ShouldBe(0);
            result.Page.ShouldBe(DefaultPage);
            result.PageSize.ShouldBe(DefaultPageSize);
        }

        private static ISchoolReadOnlyRepository CreateSchoolRepository(
            IList<School>? schools, int page, int pageSize)
        {
            var builder = new SchoolReadOnlyRepositoryBuilder();

            return schools is not null
                ? builder.WithSchoolsExist(schools, page, pageSize).Build()
                : builder.WithGetAllReturningNull(page, pageSize).Build();
        }

        private static GetAllSchoolQueryHandler CreateHandler(ISchoolReadOnlyRepository repository)
        {
            return new GetAllSchoolQueryHandler(repository);
        }
    }
}