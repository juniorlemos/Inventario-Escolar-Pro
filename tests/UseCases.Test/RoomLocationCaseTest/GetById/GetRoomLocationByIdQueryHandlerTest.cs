using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.RoomLocationRepository;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.GetById;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Test.RoomLocationCaseTest.GetById
{
    public class GetRoomLocationByIdQueryHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldReturnRoomLocationDto_WhenRoomLocationExists()
        {
            var roomLocation = RoomLocationBuilder.Build();

            var query = new GetRoomLocationByIdQuery(roomLocation.Id);

            var repository = new RoomLocationReadOnlyRepositoryBuilder()
                .WithRoomLocationExist(roomLocation.Id, roomLocation)
                .Build();

            var useCase = new GetRoomLocationByIdQueryHandler(repository);

            var result = await useCase.Handle(query, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Id.ShouldBe(roomLocation.Id);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenRoomLocationDoesNotExist()
        {
            var roomLocation = RoomLocationBuilder.Build();

            var query = new GetRoomLocationByIdQuery(roomLocation.Id);

            var repository = new RoomLocationReadOnlyRepositoryBuilder()
                .WithRoomLocationNotFound(roomLocation.Id)
                .Build();

            var useCase = new GetRoomLocationByIdQueryHandler(repository);

            var exception = await Should.ThrowAsync<NotFoundException>(() => useCase.Handle(query, CancellationToken.None));
            exception.Message.ShouldBe(ResourceMessagesException.ROOMLOCATION_NOT_FOUND);
        }
    }
}
