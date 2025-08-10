using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.RoomLocationRepository;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.GetAll;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations;
using Shouldly;

namespace UseCases.Test.RoomLocationCaseTest.GetAll
{
    public class GetAllRoomLocationQueryHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldReturnPagedResult_WhenRoomLocationsExist()
        {
            const int page = 1;
            const int pageSize = 10;
            const int totalCount = 20;
            const int expectedItems = 10;

            var roomLocations = RoomLocationBuilder.BuildList(totalCount);
            var query = new GetAllRoomLocationsQuery(page, pageSize);

            var repository = CreateRoomLocationReadOnlyRepository(true, roomLocations, page, pageSize);
            var useCase = CreateUseCase(repository);

            var result = await useCase.Handle(query, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Items.Count.ShouldBe(expectedItems);
            result.TotalCount.ShouldBe(totalCount);
            result.Page.ShouldBe(page);
            result.PageSize.ShouldBe(pageSize);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyPagedResult_WhenRepositoryReturnsNullRoomLocations()
        {
            const int page = 1;
            const int pageSize = 10;
            var query = new GetAllRoomLocationsQuery(page, pageSize);

            IList<RoomLocation>? roomLocations = null;
            var repository = CreateRoomLocationReadOnlyRepository(false, roomLocations, page, pageSize);
            var useCase = CreateUseCase(repository);

            var result = await useCase.Handle(query, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Items.ShouldBeEmpty();
            result.TotalCount.ShouldBe(0);
            result.Page.ShouldBe(page);
            result.PageSize.ShouldBe(pageSize);
        }

        private static IRoomLocationReadOnlyRepository CreateRoomLocationReadOnlyRepository(
            bool exists,
            IList<RoomLocation>? roomLocations,
            int page,
            int pageSize)
        {
            var builder = new RoomLocationReadOnlyRepositoryBuilder();

            return exists
                ? builder.WithRoomLocationsExist(roomLocations!, page, pageSize).Build()
                : builder.WithGetAllReturningNull(page, pageSize).Build();
        }

        private static GetAllRoomLocationsQueryHandler CreateUseCase(IRoomLocationReadOnlyRepository repository)
        {
            return new GetAllRoomLocationsQueryHandler(repository);
        }
    }
}