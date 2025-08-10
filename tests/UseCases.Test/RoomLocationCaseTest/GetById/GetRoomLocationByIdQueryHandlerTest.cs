using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.RoomLocationRepository;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.GetById;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.RoomLocationCaseTest.GetById
{
    public class GetRoomLocationByIdQueryHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldReturnRoomLocationDto_WhenRoomLocationExists()
        {
            var roomLocation = RoomLocationBuilder.Build();
            var query = new GetRoomLocationByIdQuery(roomLocation.Id);

            var repository = CreateRoomLocationReadOnlyRepository(true, roomLocation);
            var useCase = CreateUseCase(repository);

            var result = await useCase.Handle(query, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Id.ShouldBe(roomLocation.Id);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenRoomLocationDoesNotExist()
        {
            var roomLocation = RoomLocationBuilder.Build();
            var query = new GetRoomLocationByIdQuery(roomLocation.Id);

            var repository = CreateRoomLocationReadOnlyRepository(false, roomLocation);
            var useCase = CreateUseCase(repository);

            var exception = await Should.ThrowAsync<NotFoundException>(() => useCase.Handle(query, CancellationToken.None));
            exception.Message.ShouldBe(ResourceMessagesException.ROOMLOCATION_NOT_FOUND);
        }

        private static IRoomLocationReadOnlyRepository CreateRoomLocationReadOnlyRepository(bool exists, RoomLocation roomLocation)
        {
            var builder = new RoomLocationReadOnlyRepositoryBuilder();

            return exists
                ? builder.WithRoomLocationExist(roomLocation.Id, roomLocation).Build()
                : builder.WithRoomLocationNotExist(roomLocation.Id).Build();
        }

        private static GetRoomLocationByIdQueryHandler CreateUseCase(IRoomLocationReadOnlyRepository repository)
        {
            return new GetRoomLocationByIdQueryHandler(repository);
        }
    }
}