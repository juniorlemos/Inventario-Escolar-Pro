using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.RoomLocationRepository;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.GetAll;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Test.RoomLocationCaseTest.GetAll
{
    public class GetAllRoomLocationQueryHandlerTest
    {
        public class GetAllRoomLocationsQueryHandlerTest
        {
            [Fact]
            public async Task Handle_ShouldReturnPagedResult_WhenRoomLocationsExist()
            {
                var roomLocations = RoomLocationBuilder.BuildList(20);
                var page = 1;
                var pageSize = 10;

                var query = new GetAllRoomLocationsQuery(page, pageSize);

                var repository = new RoomLocationReadOnlyRepositoryBuilder()
                    .WithRoomLocationsExist(roomLocations, page, pageSize)
                    .Build();

                var useCase = new GetAllRoomLocationsQueryHandler(repository);

                var result = await useCase.Handle(query, CancellationToken.None);

                result.ShouldNotBeNull();
                result.Items.Count.ShouldBe(10);
                result.TotalCount.ShouldBe(20);
                result.Page.ShouldBe(page);
                result.PageSize.ShouldBe(pageSize);
            }

            [Fact]
            public async Task Handle_ShouldReturnEmptyPagedResult_WhenRepositoryReturnsNullRoomLocations()
            {
                var page = 1;
                var pageSize = 10;

                var query = new GetAllRoomLocationsQuery(page, pageSize);

                var repository = new RoomLocationReadOnlyRepositoryBuilder()
                    .WithGetAllReturningNull(page, pageSize)
                    .Build();

                var useCase = new GetAllRoomLocationsQueryHandler(repository);
                var result = await useCase.Handle(query, CancellationToken.None);

                result.ShouldNotBeNull();
                result.Items.ShouldBeEmpty();
                result.TotalCount.ShouldBe(0);
                result.Page.ShouldBe(page);
                result.PageSize.ShouldBe(pageSize);
            }
        }
    }
}