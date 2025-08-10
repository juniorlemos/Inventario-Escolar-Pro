using CommonTestUtilities.Dtos;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.RoomLocationRepository;
using FluentValidation;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.Update;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;
using Shouldly;
using static CommonTestUtilities.Helpers.CurrentUserServiceTestHelper;
using static CommonTestUtilities.Helpers.ValidatorTestHelper;

namespace UseCases.Test.RoomLocationCaseTest.Update
{
    public class UpdateRoomLocationCommandHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldUpdateRoomLocation_WhenNameIsSameAsBefore()
        {
            var roomLocation = RoomLocationBuilder.Build();
            var updateDto = UpdateRoomLocationDtoBuilder.Build();
            updateDto.Name = roomLocation.Name;

            var command = new UpdateRoomLocationCommand(roomLocation.Id, updateDto);

            var validator = CreateValidator<UpdateRoomLocationDto>(isValid: true);
            var readRepository = CreateReadOnlyRepository(true, roomLocation);
            var user = CreateCurrentUserService(true, roomLocation.SchoolId);

            var handler = CreateHandler(validator, readRepository, user);

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
        }

        [Fact]
        public async Task Handle_ShouldThrowBusinessException_WhenUserIsNotAuthenticated()
        {
            var roomLocation = RoomLocationBuilder.Build();
            var updateDto = UpdateRoomLocationDtoBuilder.Build();

            var command = new UpdateRoomLocationCommand(roomLocation.Id, updateDto);

            var validator = CreateValidator<UpdateRoomLocationDto>(isValid: true);
            var readRepository = CreateReadOnlyRepository(true, roomLocation);
            var user = CreateCurrentUserService(false);

            var handler = CreateHandler(validator, readRepository, user);

            var exception = await Should.ThrowAsync<BusinessException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ROOMLOCATION_NOT_BELONG_TO_SCHOOL);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenRoomLocationDtoIsNull()
        {
            var roomLocation = RoomLocationBuilder.Build();
            var command = new UpdateRoomLocationCommand(roomLocation.Id, null!);

            var validator = CreateValidator<UpdateRoomLocationDto>(isValid: false, ResourceMessagesException.NAME_EMPTY);
            var readRepository = CreateReadOnlyRepository(true, roomLocation);
            var user = CreateCurrentUserService(true, roomLocation.SchoolId);

            var handler = CreateHandler(validator, readRepository, user);

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.NAME_EMPTY);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFound_WhenRoomLocationDtoIsNull()
        {
            var roomLocation = RoomLocationBuilder.Build();
            var command = new UpdateRoomLocationCommand(roomLocation.Id, null!);

            var validator = CreateValidator<UpdateRoomLocationDto>(isValid: true);
            var readRepository = CreateReadOnlyRepository(false, roomLocation);
            var user = CreateCurrentUserService(true, roomLocation.SchoolId);

            var handler = CreateHandler(validator, readRepository, user);

            var exception = await Should.ThrowAsync<NotFoundException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ROOMLOCATION_NOT_FOUND);
        }

        private static IRoomLocationReadOnlyRepository CreateReadOnlyRepository(bool exists, RoomLocation roomLocation)
        {
            var builder = new RoomLocationReadOnlyRepositoryBuilder();
            return exists
                ? builder.WithRoomLocationExist(roomLocation.Id, roomLocation).Build()
                : builder.WithRoomLocationNotExist(roomLocation.Id).Build();
        }

        private static UpdateRoomLocationCommandHandler CreateHandler(
            IValidator<UpdateRoomLocationDto> validator,
            IRoomLocationReadOnlyRepository readRepository,
            ICurrentUserService user)
        {
            var unitOfWork = new UnitOfWorkBuilder().Build();
            var updateRepository = new RoomLocationUpdateOnlyRepositoryBuilder().Build();

            return new UpdateRoomLocationCommandHandler(
                unitOfWork,
                validator,
                readRepository,
                updateRepository,
                user);
        }
    }
}