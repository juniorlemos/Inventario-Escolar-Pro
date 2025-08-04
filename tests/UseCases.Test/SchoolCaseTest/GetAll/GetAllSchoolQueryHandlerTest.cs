using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.SchoolRepository;
using InventarioEscolar.Application.UsesCases.SchoolCase.GetAll;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Test.SchoolCaseTest.GetAll
{
    public class GetAllSchoolQueryHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldReturnPagedResult_WhenSchoolsExist()
        {
            var schools = SchoolBuilder.BuildList(20);
            var page = 1;
            var pageSize = 10;

            var query = new GetAllSchoolQuery(page, pageSize);

            var repository = new SchoolReadOnlyRepositoryBuilder()
                .WithSchoolsExist(schools, page, pageSize)
                .Build();

            var useCase = new GetAllSchoolQueryHandler(repository);

            var result = await useCase.Handle(query, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Items.Count.ShouldBe(10);
            result.TotalCount.ShouldBe(20);
            result.Page.ShouldBe(page);
            result.PageSize.ShouldBe(pageSize);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyPagedResult_WhenRepositoryReturnsNull()
        {
            var page = 1;
            var pageSize = 10;

            var query = new GetAllSchoolQuery(page, pageSize);

            var repository = new SchoolReadOnlyRepositoryBuilder()
                .WithGetAllReturningNull(page, pageSize)
                .Build();

            var useCase = new GetAllSchoolQueryHandler(repository);

            var result = await useCase.Handle(query, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Items.ShouldBeEmpty();
            result.TotalCount.ShouldBe(0);
            result.Page.ShouldBe(page);
            result.PageSize.ShouldBe(pageSize);
        }
    }
}
 
